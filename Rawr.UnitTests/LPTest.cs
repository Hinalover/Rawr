using Rawr.Base.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Rawr.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for LPTest and is intended
    ///to contain all LPTest Unit Tests
    ///</summary>
    [TestClass()]
    public unsafe class LPTest
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
        /// A sample LP test.
        ///</summary>
        [TestMethod()]
        public void LPSampleTest()
        {
            // maximize –2x + 5y, subject to:
            // 100 <= x <= 200  // x bounds
            // 80 <=  y <= 170  // y bounds
            // 200 <= x + y     // constraint 0

            int column;
            int rowConstraint0 = 0;

            LP.ArraySet arraySet = ArrayPool<LP.ArraySet>.RequestArraySet();

            LP lp = new LP(1, 2, arraySet);

#if !SILVERLIGHT
            fixed (double* pCost = arraySet._cost, pData = arraySet.SparseMatrixData, pValue = arraySet.SparseMatrixValue)
            fixed (int* pRow = arraySet.SparseMatrixRow, pCol = arraySet.SparseMatrixCol)
#endif
            {
#if SILVERLIGHT
                lp.BeginSafe(arraySet._cost, arraySet.SparseMatrixData, arraySet.SparseMatrixValue, arraySet.SparseMatrixRow, arraySet.SparseMatrixCol);
#else
                lp.BeginUnsafe(pCost, pData, pValue, pRow, pCol);
#endif

                // x
                column = lp.AddColumn();
                lp.SetElementUnsafe(rowConstraint0, column, 1);
                lp.SetLowerBound(column, 100); // default bounds are 0 for lower bound and infinity for upper bound
                lp.SetUpperBound(column, 200);
                lp.SetCostUnsafe(column, -2);

                // y
                column = lp.AddColumn();
                lp.SetElementUnsafe(rowConstraint0, column, 1);
                lp.SetLowerBound(column, 80);
                lp.SetUpperBound(column, 170);
                lp.SetCostUnsafe(column, 5);

                lp.EndColumnConstruction();

                // constraint bounds
                lp.SetRHS(rowConstraint0, double.PositiveInfinity);
                lp.SetLHS(rowConstraint0, 200); // if left hand side is -infinity (that is without left hand side), then this is redundant

                lp.EndUnsafe();
            }

            double[] solution = lp.SolvePrimal(false);

            // x = solution[0]
            // y = solution[1]
            // value = solution[2]

            Assert.AreEqual(100, solution[0], 0.00001);
            Assert.AreEqual(170, solution[1], 0.00001);
            Assert.AreEqual(650, solution[2], 0.00001);

            ArrayPool<LP.ArraySet>.ReleaseArraySet(arraySet);
        }
    }
}
