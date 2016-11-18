using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Amsalem.Types.CreditCards
{
    [DataContract]
    public class CreditCardCompany
    {
        [DataMember]
        public string Code = "";
        [DataMember]
        public string Name = "";

        public CreditCardCompany(string Code, string Name)
        {
            this.Code = Code;
            this.Name = Name;
        }
    }
}
