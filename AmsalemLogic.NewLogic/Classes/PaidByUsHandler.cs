using Amsalem.Types.CreditCards;
using AmsalemLogic.NewLogic.Classes.Products.ArchiveMongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmsalemLogic.VBClasses;
using AmsalemLogic.NewLogic;
using Amsalem.Types;
using AmsalemLogic.NewLogic.Entity_Framework;

namespace AmsalemLogic.NewLogic.Classes
{
    public class PaidByUsHandler
    {
        public List<CreditCard> GetAllCards()
        {
            var handler = new MongoDBHandler();
            var retriever = new AXRertrieveCreditCards();
            var cards = retriever.GetAllPaidByUsCreditCards();

            foreach (var card in cards) //Check if image exists in DB.
            {

                card.ImageExist = handler.IsImageExist(card.ComputeHashForImage());

            }

            return cards;
        }

        public ResultOfOperation CreateNewTransaction(PaidByUsTransaction transaction, ClassUsers user)
        {
            var creditcard = new CreditCard();
            var transactionLog = new PaidByUsCreditCardTransactionLog();
            var resultOfOperation = new ResultOfOperation();

            try
            {

                using (Entities db = new Entities())
                {
                    transaction.Status = (int)Amsalem.Types.Status.Pending;
                    transaction.CreatedBy = user.WorkerClockId;
                    transaction.CreatedDateTime = DateTime.Now;
                    db.PaidByUsTransaction.Add(transaction);

                    //Credit Card Operations
                    creditcard = ApllyGetCardAlgorithm(user.WorkerClockId, transaction);

                    transactionLog.ImageName = creditcard.ComputeHashForImage();
                    transactionLog.Active = true;


                    //TransactionLog Initial
                    transactionLog.PaidByUsTransactionId = transaction.Id;
                    transactionLog.CreditCardRecId = creditcard.RecId;
                    //DB Operations
                    db.PaidByUsCreditCardTransactionLog.Add(transactionLog);
                    db.SaveChanges();

                }
                resultOfOperation.Additional = transaction.Id.ToString();
                resultOfOperation.Success = true;


            }

            catch (Exception e)
            {
                resultOfOperation.Success = false;
                resultOfOperation.Additional4 = e.Message;
            }

            return resultOfOperation;
        }

        public string GetTransactionHash(PaidByUsTransaction transaction)
        { 
            var transactionLog = transaction.PaidByUsCreditCardTransactionLog.FirstOrDefault(x => x.Active.Value);
            var cardRecId = transactionLog.CreditCardRecId;

            var retriever = new AXRertrieveCreditCards();
            var card = retriever.RetrievePaidByUsSingleCreditCardByRecId(cardRecId.Value, transaction.BackOfficeCompany, EBackOfficeType.AX);

            var expirationYear = card.CreditCardExpirationDate.ToString("yy");
            var expirationMonth =   card.CreditCardExpirationDate.ToString("MM");
            var cardNumber = card.CreditCardInternalIdentifier.ToString();
            var cardHashName = cardNumber.Substring(0, 4) +
                               cardNumber.Substring(cardNumber.Length - 4, 4) +
                               expirationMonth + expirationYear;
            
            return cardHashName;
        }

        public PaidByUsTransaction RetrieveTransactionById(int id, bool activeOnly)
        {
            //TODO
            var paidByUsTransaction = new PaidByUsTransaction();
            return paidByUsTransaction;

        }

        public ResultOfOperation ReplaceCard(PaidByUsTransaction transaction, ClassUsers user, int cause)
        {

            var creditcard = new CreditCard();
            var transactionLog = new PaidByUsCreditCardTransactionLog();
            var resultOfOperation = new ResultOfOperation();
            var activeLog = transaction.PaidByUsCreditCardTransactionLog.FirstOrDefault(x => x.Active.Value);
            
            if (activeLog == null)
            {
                resultOfOperation.Message = "ActiveLog Not found";
            }
            else
            {
                activeLog.ReplacementCause = cause;
                try
                {
                    using (Entities db = new Entities())
                    {
                        var lasttransactionLog = db.PaidByUsCreditCardTransactionLog.FirstOrDefault(x => x.id == activeLog.id);
                        if (lasttransactionLog == null)
                        {
                            resultOfOperation.Message = "Active Log Not found in DB: " + activeLog.id;
                        }
                        else
                        {
                            lasttransactionLog.Active = false;
                            lasttransactionLog.ReplacementCause = cause;
                            creditcard = ApllyGetCardAlgorithm(user.WorkerClockId, transaction);
                            transactionLog.ImageName = creditcard.ComputeHashForImage();
                            transactionLog.Active = true;
                            transactionLog.PaidByUsTransactionId = transaction.Id;
                            transactionLog.CreditCardRecId = creditcard.RecId;
                            //DB Operations
                            db.PaidByUsCreditCardTransactionLog.Add(transactionLog);
                            db.SaveChanges();

                            resultOfOperation.Additional = transaction.Id.ToString();
                            resultOfOperation.Success = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    resultOfOperation.Success = false;
                    resultOfOperation.Message = e.Message;
                }
            }
            return resultOfOperation;
        }
        public CreditCard ApllyGetCardAlgorithmPubic(int userClockId, PaidByUsTransaction transaction)
        {
            return ApllyGetCardAlgorithm(userClockId, transaction);
        }
        private CreditCard ApllyGetCardAlgorithm(int userClockId, PaidByUsTransaction transaction)
        {

            var retriever = new AXRertrieveCreditCards();
            var cards = retriever.RetrievePaidByUsCreditCardsByManagerClockId(userClockId, false, "AMSA", Amsalem.Types.EBackOfficeType.AX);
            CreditCard result = null;
            try
            {
                if (transaction.Id != 0)// if there a transaction in the DB
                {
                    cards = GenerateCardListFromTransaction(transaction, cards);
                }//if trans !=0
                result = CurrencyAlgorithm(transaction, cards); 
            }

            catch (Exception ex)
            {
                throw;
            }


            return result;
        }

        private CreditCard CurrencyAlgorithm(PaidByUsTransaction transaction, List<CreditCard> cards)
        {
            var result = new CreditCard();
            var Euro = transaction.OriginalCurrencyCode == "EUR";
            if (Euro)
            {
                result = cards.Where(x => x.CreditCardType == "MC").FirstOrDefault();
                if (result == null)
                {
                    result = cards.Where(x => x.CreditCardType == "AX").FirstOrDefault();
                }

            }
            else
            {
                result = cards.Where(x => x.CreditCardType == "DI").FirstOrDefault();
                if (result == null)
                {
                    result = cards.Where(x => x.CreditCardType == "VI").FirstOrDefault();
                }
            }
            return result;
        }

        private List<CreditCard> GenerateCardListFromTransaction(PaidByUsTransaction transaction, List<CreditCard> cards)
        {
            
            cards = RemoveInvalidCards(transaction, cards);

            if (cards.Count == 0)
            {
                cards = GetDefaultCards();
                cards = RemoveInvalidCards(transaction, cards);
            }
            return cards;
        }

        private List<CreditCard> GetDefaultCards()
        {
            var retriever = new AXRertrieveCreditCards();
            var result = retriever.GetAllPaidByUsCreditCards();

            return result;
        }


        /// <summary>
        /// removes the cards that where used in the transaction.
        /// </summary>
        /// <param name="transaction">the current transacation</param>
        /// <param name="cards">list of usable credit cards</param>
        /// <returns></returns>
        private List<CreditCard> RemoveInvalidCards(PaidByUsTransaction transaction, List<CreditCard> cards)
        {
            var transactionLog = transaction.PaidByUsCreditCardTransactionLog.FirstOrDefault(x => x.Active.Value); // take the last save card for that transaction
            var cause = transactionLog.ReplacementCause;

            if (cause == 0) // if Invalid Credit Company
            {
                var card = cards.FirstOrDefault(x => x.RecId == transactionLog.CreditCardRecId);
                cards.RemoveAll(x => x.CreditCardType == card.CreditCardType);
            }
            List<CreditCard> result = null;
            var transactionLogInvalidCards = (from x in transaction.PaidByUsCreditCardTransactionLog
                                              where x.PaidByUsTransactionId.Value == transaction.Id
                                              select x.CreditCardRecId).ToList();

            result = cards.Where(x => !transactionLogInvalidCards.Contains(x.RecId)).ToList();

            //The same as : 
            //foreach (var y in transactionLogInvalidCard)
            //{
            //    foreach (CreditCard card in cards)
            //    {
            //        if (card.RecId == y)
            //        {
            //            cards.Remove(card);
            //        }
            //    }
            //}
            //result = cards;
            return result;
        }

        public PaidByUsTransaction GetTransaction(string id)
        {
            Entities db = new Entities();
            PaidByUsTransaction transaction = new PaidByUsTransaction();
            transaction = db.PaidByUsTransaction.Find(Convert.ToInt32(id));
            return transaction;
        }


    }
}
