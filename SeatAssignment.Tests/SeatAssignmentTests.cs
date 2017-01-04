using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatAssignment.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using SeatAssignment.Entities;

namespace SeatAssignment.Tests
{
    [TestClass]
    public class SeatAssignmentTests
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
        public void TestFullCapacity()
        {
            var inputReader = new FileInputReader("InputFiles\\FullHouseInput.txt");
            var requests = inputReader.GetTicketRequests();

            var theaterManager = new FairTheaterManager();

            var assignments = theaterManager.AssignSeats(requests);

            Assert.IsTrue(assignments.Count == requests.Count);
            var assignedSeats = new List<string>();
            foreach(var assignment in assignments)
            {
                assignedSeats.AddRange(assignment.AssignedSeats);
            }
            var requestedSeats = requests.Sum(x => x.NumberOfSeats);
            Assert.IsTrue(assignedSeats.Count == requestedSeats);
            Assert.IsTrue(assignedSeats.Distinct().Count() == requestedSeats);

        }
    }
}
