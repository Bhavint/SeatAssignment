using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatAssignment.BusinessLogic;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SeatAssignment.Interfaces;

namespace SeatAssignment.Tests
{
    [TestClass]
    public class SeatAssignmentTests
    {
        public static IUnityContainer container;

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
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            container = new UnityContainer().LoadConfiguration();

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

        [TestMethod]
        public void TestAllocationEfficiency()
        {
            var inputReader = new FileInputReader("InputFiles\\AllocationEfficiencyTest.txt");
            var requests = inputReader.GetTicketRequests();

            var theaterManager = container.Resolve<ITheaterManager>("internalTheaterManager");

            Assert.IsInstanceOfType(theaterManager, typeof(FairTheaterManager));

            var assignments = theaterManager.AssignSeats(requests);

            Assert.IsTrue(assignments[0].AssignedSeats.Count(seat => seat.Contains("A")) == assignments[0].AssignedSeats.Count);
            Assert.IsTrue(assignments[1].AssignedSeats.Count(seat => seat.Contains("B")) == assignments[1].AssignedSeats.Count);
            Assert.IsTrue(assignments[2].AssignedSeats.Count(seat => seat.Contains("B")) == assignments[2].AssignedSeats.Count);
            Assert.IsTrue(assignments[3].AssignedSeats.Count(seat => seat.Contains("C")) == assignments[3].AssignedSeats.Count);
            Assert.IsTrue(assignments[4].AssignedSeats.Count(seat => seat.Contains("C")) == assignments[4].AssignedSeats.Count);
            Assert.IsTrue(assignments[5].AssignedSeats.Count(seat => seat.Contains("A")) == assignments[5].AssignedSeats.Count);
        }

        [TestMethod]
        public void TestLargeGroupFavoringTheaterManager()
        {
            var inputReader = new FileInputReader("InputFiles\\AllocationEfficiencyTest.txt");
            var requests = inputReader.GetTicketRequests();

            var theaterManager = container.Resolve<ITheaterManager>("largeGroupFavoringTheaterManager");

            Assert.IsInstanceOfType(theaterManager, typeof(LargeGroupFavoringTheaterManager));

            var assignments = theaterManager.AssignSeats(requests);

            Assert.IsTrue(assignments[0].AssignedSeats.Count(seat => seat.Contains("C")) == assignments[0].AssignedSeats.Count);
            Assert.IsTrue(assignments[1].AssignedSeats.Count(seat => seat.Contains("B")) == assignments[1].AssignedSeats.Count);
            Assert.IsTrue(assignments[2].AssignedSeats.Count(seat => seat.Contains("B")) == assignments[2].AssignedSeats.Count);
            Assert.IsTrue(assignments[3].AssignedSeats.Count(seat => seat.Contains("A")) == assignments[3].AssignedSeats.Count);
            Assert.IsTrue(assignments[4].AssignedSeats.Count(seat => seat.Contains("A")) == assignments[4].AssignedSeats.Count);
            Assert.IsTrue(assignments[5].AssignedSeats.Count(seat => seat.Contains("C")) == assignments[5].AssignedSeats.Count);
        }

        [TestMethod]
        public void TestSmallGroupFavoringTheaterManager()
        {
            var inputReader = new FileInputReader("InputFiles\\AllocationEfficiencyTest.txt");
            var requests = inputReader.GetTicketRequests();

            var theaterManager = container.Resolve<ITheaterManager>("smallGroupFavoringTheaterManager");

            Assert.IsInstanceOfType(theaterManager, typeof(SmallGroupFavoringTheaterManager));

            var assignments = theaterManager.AssignSeats(requests);

            Assert.IsTrue(assignments[0].AssignedSeats.Count(seat => seat.Contains("B")) == assignments[0].AssignedSeats.Count);
            Assert.IsTrue(assignments[1].AssignedSeats.Count(seat => seat.Contains("C")) == assignments[1].AssignedSeats.Count);
            Assert.IsTrue(assignments[2].AssignedSeats.Count(seat => seat.Contains("A")) == assignments[2].AssignedSeats.Count);
            Assert.IsTrue(assignments[3].AssignedSeats.Count(seat => seat.Contains("D")) == assignments[3].AssignedSeats.Count);
            Assert.IsTrue(assignments[4].AssignedSeats.Count(seat => seat.Contains("A")) == assignments[4].AssignedSeats.Count);
            Assert.IsTrue(assignments[5].AssignedSeats.Count(seat => seat.Contains("A")) == assignments[5].AssignedSeats.Count);
        }

        [TestMethod]
        public void TestSimpleTheaterManager()
        {
            var inputReader = new FileInputReader("InputFiles\\SimpleAllocationInput.txt");
            var requests = inputReader.GetTicketRequests();

            var theaterManager = new SimpleTheaterManager();

            Assert.IsInstanceOfType(theaterManager, typeof(SimpleTheaterManager));
            var assignments = theaterManager.AssignSeats(requests);

            Assert.IsTrue(assignments[0].AssignedSeats.Count(seat => seat.Contains("A")) == 10);
            Assert.IsTrue(assignments[1].AssignedSeats.Count(seat => seat.Contains("A")) == 10);
            Assert.IsTrue(assignments[1].AssignedSeats.Count(seat => seat.Contains("B")) == 10);
            Assert.IsTrue(assignments[2].AssignedSeats.Count(seat => seat.Contains("B")) == 10);
            Assert.IsTrue(assignments[3].AssignedSeats.Count(seat => seat.Contains("C")) == 5);
            Assert.IsTrue(assignments[4].AssignedSeats.Count(seat => seat.Contains("C")) == 5);
            Assert.IsTrue(assignments[5].AssignedSeats.Count(seat => seat.Contains("C")) == 10);
            Assert.IsTrue(assignments[5].AssignedSeats.Count(seat => seat.Contains("D")) == 5);

        }
    }
}
