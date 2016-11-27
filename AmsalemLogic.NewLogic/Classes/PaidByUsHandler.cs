using Amsalem.Types.CreditCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmsalemLogic.NewLogic.Classes
{
   public class PaidByUsHandler
    {
        public  PaidByUsHandler()
        {
            
          
        }

        public List<CreditCard> BringAll()
        {
            var s = new AXRertrieveCreditCards();
            var cards = s.GetAllPaidByUsCreditCards();
            return cards;
        }
        
    }
}
