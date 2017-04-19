using System;
using AmsalemLogic.NewLogic.Entity_Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AmsalemLogic.NewLogic.Classes.Tests
{
    [TestClass]
    public class PaidByUsHandlerTests
    {
        [TestMethod]
        public void ApllyGetCardAlgorithmPubicTest()
        {
            var handler = new PaidByUsHandler();
            var transaction = new PaidByUsTransaction()
            {
                OriginalCurrencyCode = "EUR"
            };
            var card = handler.ApllyGetCardAlgorithmPubic(1332, transaction);
            Assert.IsNotNull(card);
            transaction = new PaidByUsTransaction()
            {
                OriginalCurrencyCode = "USD"
            };
            card = handler.ApllyGetCardAlgorithmPubic(1332, transaction);
            Assert.IsNotNull(card);
        }
    }
}
