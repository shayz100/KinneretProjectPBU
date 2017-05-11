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

        public ResultOfOperation CreateNewTransaction(PaidByUsTransactionDTO transaction, ClassUsers user)
        {
            var creditcard = new CreditCard();
            var resultOfOperation = new ResultOfOperation();

            try
            {
                transaction.Status = (int)Amsalem.Types.Status.Pending;
                //Credit Card Operations
                creditcard = ApllyGetCardAlgorithm(user.WorkerClockId, transaction);

                //TransactionLog Initial
                var transactionLog = new PaidByUsCreditCardTransactionLog();
                transactionLog.Active = true;
                //transactionLog.PaidByUsTransactionId = transaction.Id;
                transactionLog.CreditCardRecId = creditcard.RecId;

                transaction.PaidByUsCreditCardTransactionLog = new List<PaidByUsCreditCardTransactionLog>();

                transaction.PaidByUsCreditCardTransactionLog.Add(transactionLog);

                //DB Operations
                transaction.UpdateInDataBase(user);

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

        public string GetTransactionHash(PaidByUsTransactionDTO transaction)
        {
            string result = null;
            using (Entities db = new Entities())
            {
                
                var transactionLog = db.PaidByUsCreditCardTransactionLog.FirstOrDefault(x => x.PaidByUsTransactionId == transaction.Id && x.Active.Value == true);
                 
                //var transactionLog = transaction.PaidByUsCreditCardTransactionLog.FirstOrDefault(x => x.Active.Value);
                var cardRecId = transactionLog.CreditCardRecId;

                var retriever = new AXRertrieveCreditCards();
                var card = retriever.RetrievePaidByUsSingleCreditCardByRecId(cardRecId.Value, transaction.BackOfficeCompany, EBackOfficeType.AX);
                result = card.ComputeHashForImage();


                return result;
            }
        }

        public PaidByUsTransactionDTO RetrieveTransactionById(int id, bool activeOnly)
        {
            //TODO
            var paidByUsTransaction = new PaidByUsTransactionDTO();
            return paidByUsTransaction;

        }

        public ResultOfOperation ReplaceCard(PaidByUsTransactionDTO transaction, ClassUsers user, int cause)
        {
            var creditcard = new CreditCard();
            var transactionLog = new PaidByUsCreditCardTransactionLog();
            var resultOfOperation = new ResultOfOperation();
            Entities dblog = new Entities();
            
            var activeLog = dblog.PaidByUsCreditCardTransactionLog.FirstOrDefault(x => x.PaidByUsTransactionId == transaction.Id && x.Active.Value == true);
            
            if (activeLog == null)
            {
                resultOfOperation.Message = "Active Log Not found";
            }
            else
            {
                activeLog.ReplacementCause = cause;
                dblog.SaveChanges();
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
                            //transactionLog.ImageName = creditcard.ComputeHashForImage();
                            transactionLog.Active = true;
                            transactionLog.PaidByUsTransactionId = transaction.Id;
                            transactionLog.CreditCardRecId = creditcard.RecId;
                            //DB Operations
                            db.PaidByUsCreditCardTransactionLog.Add(transactionLog);
                            db.SaveChanges();
                            transaction.UpdateInDataBase(user);
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
        public CreditCard ApllyGetCardAlgorithmPubic(int userClockId, PaidByUsTransactionDTO transaction)
        {
            return ApllyGetCardAlgorithm(userClockId, transaction);
        }
        private CreditCard ApllyGetCardAlgorithm(int userClockId, PaidByUsTransactionDTO transaction)
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

        private CreditCard CurrencyAlgorithm(PaidByUsTransactionDTO transaction, List<CreditCard> cards)
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

            if (result == null)
            {
                cards = GetDefaultCards();
                cards = RemoveInvalidCards(transaction, cards);
                result = CurrencyAlgorithm(transaction, cards);
            }
                return result;
        }

        private List<CreditCard> GenerateCardListFromTransaction(PaidByUsTransactionDTO transaction, List<CreditCard> cards)
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
        private List<CreditCard> RemoveInvalidCards(PaidByUsTransactionDTO transaction, List<CreditCard> cards)
        {
            using (Entities db = new Entities())
            {
                var transactionLog = db.PaidByUsCreditCardTransactionLog.FirstOrDefault(x => x.PaidByUsTransactionId == transaction.Id && x.Active.Value == true);// take the last save card for that transaction
                var cause = transactionLog.ReplacementCause;

                if (cause == 0) // if Invalid Credit Company
                {
                    var card = cards.FirstOrDefault(x => x.RecId == transactionLog.CreditCardRecId);
                    cards.RemoveAll(x => x.CreditCardType == card.CreditCardType);
                }
                List<CreditCard> result = null;
                var transactionLogInvalidCards = (from x in db.PaidByUsCreditCardTransactionLog
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
        }

        public PaidByUsTransactionDTO GetTransaction(int id)
        {

            PaidByUsTransactionDTO transaction = PaidByUsTransactionDTO.Load(id);
            return transaction;
        }


    }
}
