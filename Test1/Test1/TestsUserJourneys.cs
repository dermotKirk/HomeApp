using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Rhino.Mocks;
using Test1;

namespace Test1.UserJourneys
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IMyHomeAppSystem myHomeApp; //= MockRepository.GenerateStrictMock<IMyHomeAppSystem>();
        Platform platform;
        //ITestCaseData iTestDataId;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            //IMyHomeAppSystem myHomeApp = MockRepository.GenerateStrictMock<IMyHomeAppSystem>();
            myHomeApp = MockRepository.GenerateStrictMock<IMyHomeAppSystem>();
            myHomeApp = AppInitializer.StartApp(platform);
        }

        //public void AfterEachTest()
        //{
        //    //Test Teardown code
        //}


        [Test]
        //[Test(Description =("PREPARING - SINGLE MALE - TECH SAVY - COLLEGE GRAD SAVING TO MOVE INTO HIS FIRST APARTMENT LIVING WITH PARENTS"))]
        [TestCase(TestName = "PrepareJourneySavingsWizardUsage")]
        [TestCase (100000, 20000, 24, 2000)]
        [TestCase(800000, 20000, 36, 4000)]
        //[TestCase(TestName = "PrepareJourneySavingsWizardUsage")]

        //[Description("PREPARING - SINGLE MALE - TECH SAVY - COLLEGE GRAD SAVING TO MOVE INTO HIS FIRST APARTMENT LIVING WITH PARENTS")]
        //[Category("UI")]
        //[Category("Prepare")]
        public void FTBPrepareSavingsWizardUsage(int iPropertyPrice, int iDeposit, int iSavingsTerm, int iExpectedSavingsAmount)
        {
            // ARRANGE
            //IMyHomeAppSystem myHomeApp = MockRepository.GenerateStrictMock<IMyHomeAppSystem>();
            //myHomeApp = AppInitializer.StartApp(platform);
            // app has been initialised now load test data
            //myHomeApp.LoadTestDataRequired();  ////iTestDataId = MockRepository.GenerateStub<ITestCaseData>();

            //ACT
            
            myHomeApp.AddBookmarkToJournal("BookmarkItemName1", "ItemBookMark1");
            myHomeApp.NavToJournal();
            myHomeApp.NavToJournalItem("item1");
            myHomeApp.PrepareViewJournal();
            //myHomeApp.PrepareAddItemToJournal.
            myHomeApp.PopulatePrepareSavingBudgetScreen(1);
             
            //ASSERT
            Assert.AreEqual(myHomeApp.SavingsAmount(), 200, "Amount was not correct, expected 200 but found Savings Amount was " + myHomeApp.SavingsAmount().ToString());
        }
    }
}