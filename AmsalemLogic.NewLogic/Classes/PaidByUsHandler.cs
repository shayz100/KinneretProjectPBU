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

                var cardHashName = card.CreditCardInternalIdentifier.Substring(card.CreditCardInternalIdentifier.Length - 4, 4) + 
                                   card.CreditCardInternalIdentifier.Substring(0, 4) + 
                                   card.CreditCardExpirationDate.ToString("MMyy");
                
                card.ImageExist = handler.IsImageExist(cardHashName);

            }

            return cards;
        }

        public ResultOfOperation CreateNewTransaction(PaidByUsTransaction transaction, ClassUsers user)
        {
            //TODO
            var rop = new ResultOfOperation();
            rop.Message = "Success";
            return rop;

        }

        public PaidByUsTransaction RetrieveTransactionById(int id, bool activeOnly)
        {
            //TODO
            var pbut = new PaidByUsTransaction();
            return pbut;

        }

        //orena - the function to use
        //public void CreateNewTransaction(PaidByUsTransaction transaction, ClassUsers user)
        //{
        //    var userClokId = user.WorkerClockId;

        //    var card = this.ApllyGetCardAlgorithm(userClokId, transaction);

        //    if (NewTransaction == true) // if is a new Transaction that not in the DB table.PaidByUsTransaction
        //    {
        //        SaveToPaidByUsTransaction();  // save to DB table.PaidByUsTransaction
        //    }
        //}


        public CreditCard ApllyGetCardAlgorithmPubic(int userClockId, PaidByUsTransaction transaction)
        {
            return ApllyGetCardAlgorithm(userClockId, transaction);
        }
        private CreditCard ApllyGetCardAlgorithm(int userClockId, PaidByUsTransaction transaction)
        {
            CreditCard result = null;
            try
            {
                var retriever = new AXRertrieveCreditCards();
                var cards = retriever.RetrievePaidByUsCreditCardsByManagerClockId(userClockId, false, "AMSA", Amsalem.Types.EBackOfficeType.AX);

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
                    result = this.GetDefaultCard(Euro);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        private CreditCard GetDefaultCard(bool euro)
        {
            CreditCard result = null;
            if (euro)
            {
            }
            else
            { }
            return result;
        }

    }
}
