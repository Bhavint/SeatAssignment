using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatAssignment.BusinessLogic;
using SeatAssignment.Entities;

namespace SeatAssignment.Tests
{
    /// <summary>
    /// Summary description for ValidatorTests
    /// </summary>
    [TestClass]
    public class ValidatorTests
    {

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void TestAllIncorrectInputValidation()
        {
            var requests = new List<ReservationRequest>()
            {
                new ReservationRequest("R001",-1),
                new ReservationRequest("R002", 5),
                new ReservationRequest("R002", 3),
                new ReservationRequest("R003", 200)
            };

            var validationResult = InputValidator.Validate(requests);

            Assert.IsTrue(validationResult.Status == ValidationStatus.Failure);
            Assert.IsTrue(validationResult.Messages.Count == 3);
        }

        public void TestCorrectInputValidation()
        {
            var requests = new List<ReservationRequest>()
            {
                new ReservationRequest("R001",1),
                new ReservationRequest("R002", 5),
                new ReservationRequest("R003", 3),
                new ReservationRequest("R004", 100)
            };

            var validationResult = InputValidator.Validate(requests);

            Assert.IsTrue(validationResult.Status == ValidationStatus.Success);
            Assert.IsNotNull(validationResult.Messages);
            Assert.IsTrue(validationResult.Messages.Count == 0);
        }
    }
}
