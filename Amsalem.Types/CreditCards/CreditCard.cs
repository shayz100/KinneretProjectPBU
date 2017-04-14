using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amsalem.Types;
using System.Drawing;
using System.IO;

namespace Amsalem.Types.CreditCards
{

    public class CreditCard
    {
        [Key]
        public string CreditCardNumber { get; set; }

        public PaymentMethodCreditCardOwner CreditCardOwner { get; set; }
        public string CustomerID { get; set; }
        public int Status { get; set; }
        public string ContactPersonId { get; set; }
        public string CreditCardType { get; set; }
        public string CreditCardInternalIdentifier { get; set; }
        public string OwnerName { get; set; }
        public string CreditCardIdentifier { get; set; }

        [DisplayFormat(DataFormatString = @"{0:MM\/yy}")]
        public DateTime CreditCardExpirationDate { get; set; }
        public string CreditCardNote { get; set; }
        public int BankNumber { get; set; }
        public string CreditCardOwnerID { get; set; }
        public string CreditCardAccount { get; set; }
        public string CreditCardTerminal { get; set; }
        public EBackOfficeType BackOffice { get; set; }
        public string AxCompany { get; set; }
        public bool ImageExist { get; set; }

        public string Source
        {
            get
            {
                return BackOffice.ToString();
            }
            set
            {
                EBackOfficeType backOfficeToSet = EBackOfficeType.AX;
                var parsed = Enum.TryParse(value, true, out backOfficeToSet);
                if (!parsed)
                    backOfficeToSet = EBackOfficeType.NONE;
                BackOffice = backOfficeToSet;
            }
        }

        public CreditCard()
        {
            CreditCardExpirationDate = DateTime.Now;
            CreditCardOwner = PaymentMethodCreditCardOwner.OurCC;
        }

    }
}
