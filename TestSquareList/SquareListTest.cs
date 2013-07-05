using PrototypeOne;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace TestSquareList
{
    
    
    /// <summary>
    ///This is a test class for SquareListTest and is intended
    ///to contain all SquareListTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SquareListTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for SquareList Constructor
        ///</summary>
        [TestMethod()]
        public void SquareListConstructorTest()
        {
            SquareList target = new SquareList();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            SquareList target = new SquareList(); // TODO: Initialize to an appropriate value
            Square s = null; // TODO: Initialize to an appropriate value
            target.Add(s);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for AverageLastAR
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PrototypeOne.exe")]
        public void AverageLastARTest()
        {
            SquareList_Accessor target = new SquareList_Accessor(); // TODO: Initialize to an appropriate value
            int start = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            actual = target.AverageLastAR(start, count);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AverageNewAR
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PrototypeOne.exe")]
        public void AverageNewARTest()
        {
            SquareList_Accessor target = new SquareList_Accessor(); // TODO: Initialize to an appropriate value
            int start = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            actual = target.AverageNewAR(start, count);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        public void ClearTest()
        {
            SquareList target = new SquareList(); // TODO: Initialize to an appropriate value
            target.Clear();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod()]
        public void CountTest()
        {
            SquareList target = new SquareList(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.Count();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            SquareList target = new SquareList(); // TODO: Initialize to an appropriate value
            int i = 0; // TODO: Initialize to an appropriate value
            Square expected = null; // TODO: Initialize to an appropriate value
            Square actual;
            actual = target.Get(i);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsAverageARLess
        ///</summary>
        [TestMethod()]
        public void IsAverageARLessTest()
        {
            SquareList target = new SquareList(); // TODO: Initialize to an appropriate value
            int start = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsAverageARLess(start, count);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetSameHeight
        ///</summary>
        [TestMethod()]
        public void SetSameHeightTest()
        {
            SquareList target = new SquareList(); // TODO: Initialize to an appropriate value
            int start = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            double totalWidth = 0F; // TODO: Initialize to an appropriate value
            target.SetSameHeight(start, count, totalWidth);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetSameWidth
        ///</summary>
        [TestMethod()]
        public void SetSameWidthTest()
        {
            SquareList target = new SquareList(); // TODO: Initialize to an appropriate value
            int start = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            double totalHeight = 0F; // TODO: Initialize to an appropriate value
            target.SetSameWidth(start, count, totalHeight);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for System.Collections.IEnumerable.GetEnumerator
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PrototypeOne.exe")]
        public void GetEnumeratorTest()
        {
            IEnumerable target = new SquareList(); // TODO: Initialize to an appropriate value
            IEnumerator expected = null; // TODO: Initialize to an appropriate value
            IEnumerator actual;
            actual = target.GetEnumerator();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for sublist
        ///</summary>
        [TestMethod()]
        public void sublistTest()
        {
            SquareList target = new SquareList(); // TODO: Initialize to an appropriate value
            int start = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            SquareList expected = null; // TODO: Initialize to an appropriate value
            SquareList actual;
            actual = target.sublist(start, count);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
