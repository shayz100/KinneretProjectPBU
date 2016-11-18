namespace Amsalem.Types.CreditCards
{
    public enum PaymentMethodCreditCardOwner
    {
        ClientCC,
        CompanyCC,
        OurCC,
        ThirdPartyCC,
        Fake,
        Voucher,
        WillBePayedByClient,
        Token,
        ClientAndTokenCC,
        ManualCC,
        OurGhostCC
    }

    public class PaymentMethodHelper
    {
        public static bool IsPaidByUs(PaymentMethodCreditCardOwner owner)
        {
            return owner == PaymentMethodCreditCardOwner.OurCC || owner == PaymentMethodCreditCardOwner.OurGhostCC;
        }
    }

}