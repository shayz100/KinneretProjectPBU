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

        public string GetTransactionHash(string id)
        {
            var db = new Entities();
            var intId = Convert.ToInt32(id);
            var transactionLog = new PaidByUsCreditCardTransactionLog();
            transactionLog = db
                               .PaidByUsCreditCardTransactionLog
                               .Where(x => x.PaidByUsTransactionId == intId && x.Active == true)
                               .FirstOrDefault();
            return transactionLog.ImageName;
        }

        public PaidByUsTransaction RetrieveTransactionById(int id, bool activeOnly)
        {
            //TODO
            var paidByUsTransaction = new PaidByUsTransaction();
            return paidByUsTransaction;

        }

        public ResultOfOperation ReplaceCard(PaidByUsTransaction transaction, ClassUsers user, int replacementCause)
        {

            var creditcard = new CreditCard();
            var transactionLog = new PaidByUsCreditCardTransactionLog();
            var resultOfOperation = new ResultOfOperation();
            try
            {
                using (Entities db = new Entities())
                {
                    var lasttransactionLog = db.PaidByUsCreditCardTransactionLog.OrderByDescending(x => x.PaidByUsTransactionId == transaction.Id && x.Active == true).FirstOrDefault();
                    lasttransactionLog.Active = false;
                    lasttransactionLog.ReplacementCause = replacementCause;
                    db.SaveChanges();

                    creditcard = ApllyGetCardAlgorithm(user.WorkerClockId, transaction);
                    transactionLog.ImageName = creditcard.ComputeHashForImage();
                    transactionLog.Active = true;
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
                if (transaction.Id > 1)
                {
                    using (Entities db = new Entities())
                    {

                        var transactionLog = db.PaidByUsCreditCardTransactionLog.OrderByDescending(x => x.PaidByUsTransactionId == transaction.Id).FirstOrDefault(); // take the last save card for that transaction

                        var cause = transactionLog.ReplacementCause;
                        List<PaidByUsCreditCardTransactionLog> list = new List<PaidByUsCreditCardTransactionLog>();
                        List<CreditCard> dfaultcards = null;

                        if (cause == 0) // if Invalid Credit Company
                        {
                            var card = cards.Where(x => x.RecId == transactionLog.CreditCardRecId).FirstOrDefault();
                            cards.RemoveAll(x => x.CreditCardType == card.CreditCardType);
                            if (cards.Count == 0)
                            {
                                dfaultcards = GetDefaultCard();
                                cards = RemoveInvalidCard(transaction, dfaultcards);
                            }
                        }
                        else if (cause == 1) // if Invalid Card
                        {
                            cards = RemoveInvalidCard(transaction, cards);

                            if (cards.Count == 0)
                            {
                                dfaultcards = GetDefaultCard();
                                cards = RemoveInvalidCard(transaction, dfaultcards);
                            }
                        }
                    }
                }
                else
                {
                }

                var Euro = transaction.OriginalCurrencyCode == "EUR";
                if (Euro)
                {
                    result = cards.Where(x => x.CreditCardType == "MC").FirstOrDefault();
                    if (result == null)
                    {
                        result = cards.Where(x => x.CreditCardType == "AX").FirstOrDefault();
                    }

                    if (result == null)
                    {
                        result = cards.Where(x => x.CreditCardType == "DI").FirstOrDefault();
                    }


                    if (result == null)
                    {
                        result = cards.Where(x => x.CreditCardType == "VI").FirstOrDefault();
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

            }

            catch (Exception ex)
            {
                throw;
            }


            return result;
        }

        private List<CreditCard> GetDefaultCard()
        {
            var retriever = new AXRertrieveCreditCards();
            var result = retriever.GetAllPaidByUsCreditCards();

            return result;
        }

        private List<CreditCard> RemoveInvalidCard(PaidByUsTransaction transaction, List<CreditCard> cards)
        {
            List<CreditCard> result = null;

            using (Entities db = new Entities())
            {
                var transactionLogInvalidCard = from x in db.PaidByUsCreditCardTransactionLog
                                                where x.PaidByUsTransactionId.Value == transaction.Id
                                                select x.CreditCardRecId;

                foreach (var y in transactionLogInvalidCard)
                {
                    foreach (CreditCard card in cards.ToList())
                    {
                        if (card.RecId == y)
                        {
                            cards.Remove(card);
                        }
                    }
                }
                result = cards;
            }
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
