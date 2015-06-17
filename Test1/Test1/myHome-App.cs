using System;
using System.Linq;
using System.Text;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using NUnit;


namespace Test1
{

    public interface IMyHomeAppSystem_Journal
    {

        //Nav through app actions
        IMyHomeAppSystem NavToPrepareScreen();
        IMyHomeAppSystem NavToLookingScreen();
        IMyHomeAppSystem NavToApplyingScreen();
        IMyHomeAppSystem NavToBuyingScreen();

        int SavingsAmount();

        //Prepare screens available
        IMyHomeAppSystem PrepareViewJournal();
        IMyHomeAppSystem PrepareViewCheckList();
        IMyHomeAppSystem PrepareAddItemToJournal(string Item, string ItemType);
        IMyHomeAppSystem PrepareCheckItemsInCheckList();
        IMyHomeAppSystem LoadTestDataRequired();
        IMyHomeAppSystem Cancel();
        IMyHomeAppSystem PopulatePrepareSavingBudgetScreen(int testDataID);
        //  FTB (first/second time buyer)or STB what type of buyer are they as rules are different
        //  Saving timeline frequency (12, 24, 36 months)
        //  PP Purchase Price of house required
        //  Deposit Goal amount
        //  Savings / Gift available
        // Based on the above details we should be able to give a monthly savings required
        // figure to the user within the Budget Calculator.



        //Looking screen actions
        IMyHomeAppSystem SaveFavouriteHouse(int HouseId);
        IMyHomeAppSystem ViewFavouriteHouse(int HouseId);
        IMyHomeAppSystem DeleteFavouriteHouse(int HouseId);
        IMyHomeAppSystem LookingViewCheckList();
        IMyHomeAppSystem LookingAddItemToJournal(string Item, string ItemType);
        IMyHomeAppSystem LookingUpdateItemsInCheckListCheck(string ItemsToCheck);
        IMyHomeAppSystem LookingUpdateItemsInCheckListUnCheck(string ItemsToUnCheck);
        IMyHomeAppSystem LookingViewJournal();
        IMyHomeAppSystem LookingThroughToAskAIB();
        IMyHomeAppSystem PopulateLookingSavingBudgetScreen(int testDataID);
        //    using below details we can provide a stressed payment amount budget
        //    Already know TFB or STB - no need to display on screen
        //    Already know the house price value- prepopulated on a slider from previous buget info if
        //        already available
        //    Deposit amount
        //    Monthly savings amount on slider
        //    Monthly Rent (Out goings)
        //    Mortgage Term in Years (Max of 35 years) no logic around age viability as no DOB
        //        given at any point.
        //    Interest rate is selectable
        //    Warning given if out goings greater thna incoming(RED),
        //        Apply For Mortgage link shown if NOT(GREEN).


        //Applying Screen actions
        IMyHomeAppSystem ApplyViewJournal();
        IMyHomeAppSystem ApplyAddItemToJournal(string Item, string ItemType);
        IMyHomeAppSystem ApplyViewCheckList();
        IMyHomeAppSystem ApplyCheckItemsInCheckList();
        IMyHomeAppSystem ApplyingView_VideoLinksAvailable();
        IMyHomeAppSystem ApplyingView_EachOfTheVideoLinksAvailable();
        IMyHomeAppSystem ApplyingView_NewsLinksAvailable();
        IMyHomeAppSystem ApplyingView_Favourites();
        IMyHomeAppSystem ApplyingView_PropertyInformation();
        IMyHomeAppSystem ApplyingView_PropertyAmenityInformation();
        IMyHomeAppSystem ApplyingSelectFiltersAndApply(int testData);
        IMyHomeAppSystem ApplyingSelectP1FiltersCustomerSearchCriteria(int testData);
        IMyHomeAppSystem ApplyingSelectP2Filters(int testData);
        IMyHomeAppSystem ApplyingSelectP3Filters(int testData);
        IMyHomeAppSystem ApplyingAddFavouriteAreas(int testData);
        IMyHomeAppSystem ApplyingAddFavouriteProperty(int testData);
        IMyHomeAppSystem ApplyingAddFavouriteAlert(int testData);
        IMyHomeAppSystem ApplyingViewSupportingDocumentsChecker();
        IMyHomeAppSystem ApplyingCheckOffSupportingDocumentInDocChecker();
        IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSonline();
        IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSoffline();
        IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSON();
        IMyHomeAppSystem ApplyingViewSpeakToMortgageAdvisor();
        IMyHomeAppSystem ApplyingViewVideoLinksAvailable();
        IMyHomeAppSystem ApplyingViewEachOfTheVideoLinksAvailable();
        IMyHomeAppSystem ApplyingClickThroughToOnlineApplication();
        IMyHomeAppSystem ApplyingClickThroughToAskAIB();
        IMyHomeAppSystem PopulateApplyMortgageBudgetScreen(int testDataID);

        //Moving-In actions
        IMyHomeAppSystem MovingInViewJournal();
        IMyHomeAppSystem MovingInCheckItemsInCheckList();
        IMyHomeAppSystem MovingInSurveyorLocatorGPSonline();
        IMyHomeAppSystem MovingInSurveyorLocatorGPSoffline();
        IMyHomeAppSystem MovingInValuerLocatorGPSonline();
        IMyHomeAppSystem MovingInValuerLocatorGPSoffline();
        IMyHomeAppSystem MovingInSolicitorLocatorGPSonline();
        IMyHomeAppSystem MovingInSolicitorLocatorGPSoffline();
        IMyHomeAppSystem MovingInViewSurveyorDetails();
        IMyHomeAppSystem MovingInValuerDetails();
        IMyHomeAppSystem MovingInSolicitorDetails();
        IMyHomeAppSystem MovingInTopTips();
        IMyHomeAppSystem MovingInNewsFeed();
        IMyHomeAppSystem MovingInMyHome();
        IMyHomeAppSystem MovingInAddItemToJournal(string Item, string ItemType);
        IMyHomeAppSystem MovingInViewCheckList();
        IMyHomeAppSystem PopulateMovingInBudgetScreen(int testDataID);


        //Journal Screen Actions
        IMyHomeAppSystem AddContactToJournal(string Item, string sContact);
        IMyHomeAppSystem AddPictureToJournal(string Item, string sPicture);
        IMyHomeAppSystem AddBookmarkToJournal(string Item, string sBookmark);
        IMyHomeAppSystem AddSavedVideosToJournal(string Item, string sSavedVideo);

        //bool HasItem(string itemName);
    }



    public interface IMyHomeAppSystem
    {
        
        //Nav through app actions
        IMyHomeAppSystem NavToPrepareScreen();
        IMyHomeAppSystem NavToLookingScreen();
        IMyHomeAppSystem NavToApplyingScreen();
        IMyHomeAppSystem NavToBuyingScreen();

        int SavingsAmount();

        //Prepare screens available
        IMyHomeAppSystem PrepareViewJournal();
        IMyHomeAppSystem PrepareViewCheckList();
        IMyHomeAppSystem PrepareAddItemToJournal(string Item, string ItemType);
        IMyHomeAppSystem PrepareCheckItemsInCheckList();
        IMyHomeAppSystem LoadTestDataRequired();
        IMyHomeAppSystem Cancel();
        IMyHomeAppSystem PopulatePrepareSavingBudgetScreen(int testDataID);
        //  FTB (first/second time buyer)or STB what type of buyer are they as rules are different
        //  Saving timeline frequency (12, 24, 36 months)
        //  PP Purchase Price of house required
        //  Deposit Goal amount
        //  Savings / Gift available
        // Based on the above details we should be able to give a monthly savings required
        // figure to the user within the Budget Calculator.



        //Looking screen actions
        IMyHomeAppSystem SaveFavouriteHouse(int HouseId);
        IMyHomeAppSystem ViewFavouriteHouse(int HouseId);
        IMyHomeAppSystem DeleteFavouriteHouse(int HouseId);
        IMyHomeAppSystem LookingViewCheckList();
        IMyHomeAppSystem LookingAddItemToJournal(string Item, string ItemType);
        IMyHomeAppSystem LookingUpdateItemsInCheckListCheck(string ItemsToCheck);
        IMyHomeAppSystem LookingUpdateItemsInCheckListUnCheck(string ItemsToUnCheck);
        IMyHomeAppSystem LookingViewJournal();
        IMyHomeAppSystem LookingThroughToAskAIB();
        IMyHomeAppSystem PopulateLookingSavingBudgetScreen(int testDataID);
        //    using below details we can provide a stressed payment amount budget
        //    Already know TFB or STB - no need to display on screen
        //    Already know the house price value- prepopulated on a slider from previous buget info if
        //        already available
        //    Deposit amount
        //    Monthly savings amount on slider
        //    Monthly Rent (Out goings)
        //    Mortgage Term in Years (Max of 35 years) no logic around age viability as no DOB
        //        given at any point.
        //    Interest rate is selectable
        //    Warning given if out goings greater thna incoming(RED),
        //        Apply For Mortgage link shown if NOT(GREEN).


        //Applying Screen actions
        IMyHomeAppSystem ApplyViewJournal();
        IMyHomeAppSystem ApplyAddItemToJournal(string Item, string ItemType);
        IMyHomeAppSystem ApplyViewCheckList();
        IMyHomeAppSystem ApplyCheckItemsInCheckList();
        IMyHomeAppSystem ApplyingView_VideoLinksAvailable();
        IMyHomeAppSystem ApplyingView_EachOfTheVideoLinksAvailable();
        IMyHomeAppSystem ApplyingView_NewsLinksAvailable();
        IMyHomeAppSystem ApplyingView_Favourites();
        IMyHomeAppSystem ApplyingView_PropertyInformation();
        IMyHomeAppSystem ApplyingView_PropertyAmenityInformation();
        IMyHomeAppSystem ApplyingSelectFiltersAndApply(int testData);
        IMyHomeAppSystem ApplyingSelectP1FiltersCustomerSearchCriteria(int testData);
        IMyHomeAppSystem ApplyingSelectP2Filters(int testData);
        IMyHomeAppSystem ApplyingSelectP3Filters(int testData);
        IMyHomeAppSystem ApplyingAddFavouriteAreas(int testData);
        IMyHomeAppSystem ApplyingAddFavouriteProperty(int testData);
        IMyHomeAppSystem ApplyingAddFavouriteAlert(int testData);
        IMyHomeAppSystem ApplyingViewSupportingDocumentsChecker();
        IMyHomeAppSystem ApplyingCheckOffSupportingDocumentInDocChecker();
        IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSonline();
        IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSoffline();
        IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSON();
        IMyHomeAppSystem ApplyingViewSpeakToMortgageAdvisor();
        IMyHomeAppSystem ApplyingViewVideoLinksAvailable();
        IMyHomeAppSystem ApplyingViewEachOfTheVideoLinksAvailable();
        IMyHomeAppSystem ApplyingClickThroughToOnlineApplication();
        IMyHomeAppSystem ApplyingClickThroughToAskAIB();
        IMyHomeAppSystem PopulateApplyMortgageBudgetScreen(int testDataID);

        //Moving-In actions
        IMyHomeAppSystem MovingInViewJournal();
        IMyHomeAppSystem MovingInCheckItemsInCheckList();
        IMyHomeAppSystem MovingInSurveyorLocatorGPSonline();
        IMyHomeAppSystem MovingInSurveyorLocatorGPSoffline();
        IMyHomeAppSystem MovingInValuerLocatorGPSonline();
        IMyHomeAppSystem MovingInValuerLocatorGPSoffline();
        IMyHomeAppSystem MovingInSolicitorLocatorGPSonline();
        IMyHomeAppSystem MovingInSolicitorLocatorGPSoffline();
        IMyHomeAppSystem MovingInViewSurveyorDetails();
        IMyHomeAppSystem MovingInValuerDetails();
        IMyHomeAppSystem MovingInSolicitorDetails();
        IMyHomeAppSystem MovingInTopTips();
        IMyHomeAppSystem MovingInNewsFeed();
        IMyHomeAppSystem MovingInMyHome();
        IMyHomeAppSystem MovingInAddItemToJournal(string Item, string ItemType);
        IMyHomeAppSystem MovingInViewCheckList();
        IMyHomeAppSystem PopulateMovingInBudgetScreen(int testDataID);


        //Journal Screen Actions
        IMyHomeAppSystem AddContactToJournal(string Item, string sContact);
        IMyHomeAppSystem AddPictureToJournal(string Item, string sPicture);
        IMyHomeAppSystem AddBookmarkToJournal(string Item, string sBookmark);
        IMyHomeAppSystem AddSavedVideosToJournal(string Item, string sSavedVideo);

        //bool HasItem(string itemName);
    }


    public class AndroidmyHome_App : IMyHomeAppSystem
    {
        IApp app;

        public AndroidmyHome_App(IApp app) 
            {
            this.app = app;
            }

        public IMyHomeAppSystem AddBookmarkToJournal(string Item, string sBookmark)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem AddContactToJournal(string Item, string sContact)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem AddPictureToJournal(string Item, string sPicture)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem AddSavedVideosToJournal(string Item, string sSavedVideo)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyAddItemToJournal(string Item, string ItemType)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyCheckItemsInCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingAddFavouriteAlert(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingAddFavouriteAreas(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingAddFavouriteProperty(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingCheckOffSupportingDocumentInDocChecker()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingClickThroughToAskAIB()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingClickThroughToOnlineApplication()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingSelectFiltersAndApply(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingSelectP1FiltersCustomerSearchCriteria(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingSelectP2Filters(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingSelectP3Filters(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewEachOfTheVideoLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSoffline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSON()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSonline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewSpeakToMortgageAdvisor()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewSupportingDocumentsChecker()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewVideoLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_EachOfTheVideoLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_Favourites()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_NewsLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_PropertyAmenityInformation()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_PropertyInformation()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_VideoLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyViewCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyViewJournal()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem Cancel()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem DeleteFavouriteHouse(int HouseId)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LoadTestDataRequired()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingAddItemToJournal(string Item, string ItemType)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingThroughToAskAIB()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingUpdateItemsInCheckListCheck(string ItemsToCheck)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingUpdateItemsInCheckListUnCheck(string ItemsToUnCheck)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingViewCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingViewJournal()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInAddItemToJournal(string Item, string ItemType)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInCheckItemsInCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInMyHome()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInNewsFeed()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSolicitorDetails()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSolicitorLocatorGPSoffline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSolicitorLocatorGPSonline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSurveyorLocatorGPSoffline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSurveyorLocatorGPSonline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInTopTips()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInValuerDetails()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInValuerLocatorGPSoffline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInValuerLocatorGPSonline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInViewCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInViewJournal()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInViewSurveyorDetails()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem NavToApplyingScreen()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem NavToBuyingScreen()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem NavToLookingScreen()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem NavToPrepareScreen()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PopulateApplyMortgageBudgetScreen(int testDataID)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PopulateLookingSavingBudgetScreen(int testDataID)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PopulateMovingInBudgetScreen(int testDataID)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PopulatePrepareSavingBudgetScreen(int testDataID)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PrepareAddItemToJournal(string Item, string ItemType)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PrepareCheckItemsInCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PrepareViewCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PrepareViewJournal()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem SaveFavouriteHouse(int HouseId)
        {
            throw new NotImplementedException();
        }

        public int SavingsAmount()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ViewFavouriteHouse(int HouseId)
        {
            throw new NotImplementedException();
        }
    }

    public class IOSmyHome_App : IMyHomeAppSystem
    {
        IApp app;

        public IOSmyHome_App(IApp app)
        {
            this.app = app;
        }

        public IMyHomeAppSystem AddBookmarkToJournal(string Item, string sBookmark)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem AddContactToJournal(string Item, string sContact)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem AddPictureToJournal(string Item, string sPicture)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem AddSavedVideosToJournal(string Item, string sSavedVideo)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyAddItemToJournal(string Item, string ItemType)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyCheckItemsInCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingAddFavouriteAlert(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingAddFavouriteAreas(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingAddFavouriteProperty(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingCheckOffSupportingDocumentInDocChecker()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingClickThroughToAskAIB()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingClickThroughToOnlineApplication()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingSelectFiltersAndApply(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingSelectP1FiltersCustomerSearchCriteria(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingSelectP2Filters(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingSelectP3Filters(int testData)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewEachOfTheVideoLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSoffline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSON()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewMortgageAdvisorLocatorwithGPSonline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewSpeakToMortgageAdvisor()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewSupportingDocumentsChecker()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingViewVideoLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_EachOfTheVideoLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_Favourites()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_NewsLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_PropertyAmenityInformation()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_PropertyInformation()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyingView_VideoLinksAvailable()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyViewCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ApplyViewJournal()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem Cancel()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem DeleteFavouriteHouse(int HouseId)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LoadTestDataRequired()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingAddItemToJournal(string Item, string ItemType)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingThroughToAskAIB()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingUpdateItemsInCheckListCheck(string ItemsToCheck)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingUpdateItemsInCheckListUnCheck(string ItemsToUnCheck)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingViewCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem LookingViewJournal()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInAddItemToJournal(string Item, string ItemType)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInCheckItemsInCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInMyHome()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInNewsFeed()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSolicitorDetails()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSolicitorLocatorGPSoffline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSolicitorLocatorGPSonline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSurveyorLocatorGPSoffline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInSurveyorLocatorGPSonline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInTopTips()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInValuerDetails()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInValuerLocatorGPSoffline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInValuerLocatorGPSonline()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInViewCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInViewJournal()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem MovingInViewSurveyorDetails()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem NavToApplyingScreen()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem NavToBuyingScreen()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem NavToLookingScreen()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem NavToPrepareScreen()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PopulateApplyMortgageBudgetScreen(int testDataID)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PopulateLookingSavingBudgetScreen(int testDataID)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PopulateMovingInBudgetScreen(int testDataID)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PopulatePrepareSavingBudgetScreen(int testDataID)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PrepareAddItemToJournal(string Item, string ItemType)
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PrepareCheckItemsInCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PrepareViewCheckList()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem PrepareViewJournal()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem SaveFavouriteHouse(int HouseId)
        {
            throw new NotImplementedException();
        }

        public int SavingsAmount()
        {
            throw new NotImplementedException();
        }

        public IMyHomeAppSystem ViewFavouriteHouse(int HouseId)
        {
            throw new NotImplementedException();
        }
    }

}
