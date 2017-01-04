using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SeatAssignment.Interfaces;
using SeatAssignment.Entities;
using System.IO;

namespace SeatAssignment.Tests
{
    
    [TestClass]
    public class OutputWriterTests
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
        public void TestEmptySeatAssignment()
        {
            var filePath = "output";
            var outputWriter = container.Resolve<IOutputWriter>(new ResolverOverride[] { new ParameterOverride("filePath", filePath) });
            Assert.IsTrue(outputWriter.GenerateOutput(new List<ReservationAssignment>()));

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Assert.IsNotNull(fileStream);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                var line = streamReader.ReadLine();
                Assert.IsTrue(string.IsNullOrEmpty(line));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Error Occured while writing to file")]
        public void TestIncorrectOutputFilePath()
        {
            var filePath = "J:\\";
            var outputWriter = container.Resolve<IOutputWriter>(new ResolverOverride[] { new ParameterOverride("filePath", filePath) });
            outputWriter.GenerateOutput(new List<ReservationAssignment>());
        }
    }
}
