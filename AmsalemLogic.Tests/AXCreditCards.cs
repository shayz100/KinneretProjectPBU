using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amsalem.Types.CreditCards;

namespace AmsalemLogic.Tests
{
    [TestClass]
    public class AXCreditCards
    {
        [TestMethod]
        public void GetAxCards()
        {
            var retreive = new AXRertrieveCreditCards();
            var result = retreive.GetAllPaidByUsCreditCards();
            Assert.IsTrue(result.Count > 0);
        }
    }
}
