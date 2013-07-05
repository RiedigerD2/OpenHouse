using PrototypeOne;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Media;
namespace TestProject1
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
        ///A test for SetSameHeight
        ///</summary>
        [TestMethod()]
        public void SetSameHeightTest()
        {
            Square one;
            FillInfo oneFill = new FillInfo(Brushes.Aqua);
            Square two;
            FillInfo twoFill = new FillInfo(Brushes.Aqua);
            Square three;
            FillInfo threeFill = new FillInfo(Brushes.Aqua);
            Square four;
            FillInfo fourFill = new FillInfo(Brushes.Aqua);
            SquareList thislist = new SquareList();
            one = new Square(4.8, oneFill);
            two = new Square(3.6, twoFill);
            three = new Square(2.4, threeFill);
            four = new Square(1.2, fourFill);
            thislist.Add(one);
            thislist.Add(two); 
            thislist.Add(three); 
            thislist.Add(four);

            
            Console.Out.WriteLine(one.ToString()+" lastCount:"+thislist.Count()+"\n\n");
            for (int i = 0; i < 4; i++) {
                Console.Out.WriteLine(thislist.Get(i).ToString()+"\nDone\n\n");
            }
            //begin testing
            thislist.SetSameHeight(0, 3, 4);
            thislist.SetSameHeight(0, 3, 4);
            for (int i = 1; i < 3; i++)
            {
                Assert.AreEqual(thislist.Get(0).Height, thislist.Get(i).Height);
            }
            double sum=0,expected=4;
            for (int i = 0; i < 3; i++)
            {
                sum+=thislist.Get(i).Width;
            }
            Assert.AreEqual(4.0, sum);
          
           // Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        
    }
}
