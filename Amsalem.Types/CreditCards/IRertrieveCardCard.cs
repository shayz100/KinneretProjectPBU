using System.Collections.Generic;

namespace Amsalem.Types.CreditCards
{
    public interface IRetrieveCardCard
    {
        
        List<CreditCard> RetrievePaidByUsCreditCardsByManagerClockId(int OnBehalfOfClockID, bool filterd, string dataAreaID, EBackOfficeType backOffice, bool withVAN = false);
       
        CreditCard RetrievePaidByUsSingleCreditCard(string CreditCardNumber, string dataAreaID, EBackOfficeType backOffice);
    }

    public abstract class RetrieveCreditCardBase
    {
        public bool IgnoreExpirationDate { get; set; }
    }
}
