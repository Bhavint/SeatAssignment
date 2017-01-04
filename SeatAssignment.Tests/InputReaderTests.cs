using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SeatAssignment.Interfaces;

namespace SeatAssignment.Tests
{
    [TestClass]
    public class InputReaderTests
    {
        public static IUnityContainer container;

        private TestContext _testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            container = new UnityContainer().LoadConfiguration();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Error occured while reading from file")]
        public void TestIncorrectFilePath()
        {
            var inputReader = container.Resolve<IInputReader>(new ResolverOverride[] { new ParameterOverride("filePath", "HopiPola") });
            var requests = inputReader.GetTicketRequests();
        }

        [TestMethod]
        public void TestEmptyFileInput()
        {
            var filePath = "InputFiles\\EmptyInput.txt";
            var inputReader = container.Resolve<IInputReader>(new ResolverOverride[] { new ParameterOverride("filePath", filePath) });
            var requests = inputReader.GetTicketRequests();
            Assert.IsTrue(requests != null);
            Assert.IsTrue(requests.Count == 0);
        }

        [TestMethod]
        public void TestCorrectInput()
        {
            var filePath = "InputFiles\\5linesInput.txt";
            var inputReader = container.Resolve<IInputReader>(new ResolverOverride[] { new ParameterOverride("filePath", filePath) });
            var requests = inputReader.GetTicketRequests();
            Assert.IsTrue(requests != null);
            Assert.IsTrue(requests.Count == 5);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Error occured while reading from file")]
        public void TestIncorrectInput()
        {
            var filePath = "InputFiles\\IncorrectInput.txt";
            var inputReader = container.Resolve<IInputReader>(new ResolverOverride[] { new ParameterOverride("filePath", filePath) });
            var requests = inputReader.GetTicketRequests();
        }
    }
}
