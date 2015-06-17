using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

using System.Net.Http;
using System.Net.Http.Formatting;

namespace Test1
{
    public interface ITestAction
    {
        void BeforeTest(TestDetails details);

        void AfterTest(TestDetails details);

        ActionTargets Targets { get; }
    }



    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class |
                    AttributeTargets.Interface | AttributeTargets.Assembly,
                    AllowMultiple = true)]
    public class ConsoleActionAttribute : Attribute, ITestAction
    {
        private string _Message;

        public ConsoleActionAttribute(string message) { _Message = message; }

        public void BeforeTest(TestDetails details)
        {
            WriteToConsole("Before", details);
        }

        public void AfterTest(TestDetails details)
        {
            WriteToConsole("After", details);
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Test | ActionTargets.Suite; }
        }

        private void WriteToConsole(string eventMessage, TestDetails details)
        {
            Console.WriteLine("{0} {1}: {2}, from {3}.{4}.",
                eventMessage,
                details.IsSuite ? "Suite" : "Case",
                _Message,
                details.Fixture != null ? details.Fixture.GetType().Name : "{no fixture}",
                details.Method != null ? details.Method.Name : "{no method}");
        }
    }




    public class RestClient
    {
        private HttpClient _client = null;

        public HttpClient client(string uri)
        {
            // Write action to console
            Console.WriteLine("No active HttpClient, creating a new one.");

            // Create new Http Client Handler
            HttpClientHandler handler = new HttpClientHandler
            {
                // Do not use a proxy
                UseProxy = false
            };

            // Create new Http Client using the Handler
            _client = new HttpClient(handler);

            // Set BaseAddress
            _client.BaseAddress = new Uri(uri);

            // Write action to console
            Console.WriteLine("Set Base Address to " + _client.BaseAddress.ToString());
            return _client;
        }
    }


    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IMyHomeAppSystem myHomeApp;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            myHomeApp = AppInitializer.StartApp(platform);
        }


        //[Test]
        //public void TaskyPro_CreatingATask_ShouldBeSuccessful()
        //{
        //    //using new inteface to the test harness
        //    myHomeApp.Add()
        //        .SetName("Get Milk")
        //        .SetNotes("Pickup Milk")
        //        .Save();

        //    Assert.IsTrue(myHomeApp.HasItem("Get Milk"));
        //}

        //[Test]
        //public void TaskyPro_DeletingATask_ShouldBeSuccessful()
        //{
        //    //  New code using interface for cross platform test
        //    myHomeApp.Add()
        //    .SetName("Test Delete")
        //    .SetNotes("This item should be deleted")
        //    .Save();
        //    //app.WaitForNoElement(c => c.Marked(name), "Timed out", TimeSpan.FromSeconds(3));
        //    myHomeApp.Delete("Test Delete");

        //    Assert.IsFalse(myHomeApp.HasItem("Test Delete"));
        //}
    }

    [TestFixture]
    public class MainEngineTests
    {
        [SetUp]
        //need to setup the HTTP client and server details to test REST calls
        public void BeforeEachTest()
        {
            //define the server details.
            //define the HTTP client
            //Expected results are configured
        }
        [TearDown]
        public void AfterEachTest()
        {
            //Close the HTTP client for REST API call
        }

        [Test]
        [Category("System")]
        //[ExpectedException(typeof(SystemException))]
        //[ExpectedException("InsufficientFundsException")]
        public void API_isAvailable()
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://www.thomas-bayer.com/sqlrest/CUSTOMER/");
            // is the server available??            
            Assert.IsFalse(((client.ToString().Length > 0) == false), "system available");
        }

        [Test]
        [Category("PersonListings")]
        public void API_PersonsListIsAvailable()
        {
            // is the server available??
            Assert.IsFalse(false, "system available");
            // make a call the list of persons ok
            Assert.IsFalse(true, "list of plp available!");
            // make a call to see a specific person
            Assert.IsFalse(true, "Specific person is available");
        }

        [Test]
        [Category("SpecificPerson")]
        [Ignore("Not available in this version")]
        public void API_SpecificPersonDetailsIsAvailable()
        {
            // is the server available??
            Assert.IsFalse(false, "system available");
            // make a call the list of persons ok
            Assert.IsFalse(true, "list of plp available!");
            // make a call to see a specific person
            Assert.IsFalse(true, "Specific person is available");
        }

        [Test]
        [Category("API")]
        [Category("Property")]
        [Ignore("Not available in this version")]
        public void API_ListAProperty()
        {
            Assert.IsFalse(true, "Property information not available");
        }

        [Test]
        [Category("API")]
        [Category("Property")]
        [Ignore("Not available in this version")]
        public void API_ListAProperty_Information()
        {
            Assert.IsFalse(true, "No property Information available");
        }

        [Test]
        [Category("API")]
        [Category("Filters")]
        [Ignore("Not available in this version")]
        public void API_ListPropertyFiltersAvailable()
        {
            Assert.IsFalse(true, "No Filters available");
        }

        [Test]
        [Category("API")]
        [Category("Filters")]
        [Ignore("Not available in this version")]
        public void API_ListOfPropertiesBasedOnFilters()
        {
            Assert.IsFalse(true, "No list of properites returned for filters selected");
        }

        [Test]
        [Category("API")]
        [Category("Favs")]
        [Ignore("Not available in this version")]
        public void API_AddPropertyToUsersFav()
        {
            Assert.IsFalse(true, "Could not add Users Fav");
        }

        [Test]
        [Category("API")]
        [Category("Favs")]
        //[Ignore("Not available in this version")]
        public void API_ListUsersFavProperties()
        {
            Assert.IsFalse(true, "User Favs didn;t load");
        }

        [Test]
        [Category("GeoCord")]
        [Category("RadiusSearch")]
        //[Ignore("Not available in this version")]
        public void API_GeoCord_RadiusSearchFromSpirePin()
        {
            Assert.IsFalse(true, "GeoCord Radius Search didn't work");
        }

        [Test]
        [Category("GeoCord")]
        [Category("RadiusSearch")]
        //[Ignore("Not available in this version")]
        public void UI_GeoCord_RadiusSearchFromSpirePin()
        {
            Assert.IsFalse(true, "UI didn;t show GeoCord Radius Search didn't work");
        }
    }

    [TestFixture]
    public class ImporterTest
    {
        [Test]
        [Category("Importer")]
        [Category("Daft")]
        public void Importer_Daft()
        {
            //CategoryAttribute ca = this;
            Assert.IsTrue(false, this.ToString());
        }

        [Test]
        [Category("Importer")]
        [Category("MyHome")]
        public void Importer_MyHome()
        {

        }

        [Test]
        [Category("Importer")]
        [Category("GeoDirectory")]
        [Category("Exception")]
        public void Importer_GeoDirectory()
        {

        }

        [Test]
        [Category("Importer")]
        [Category("GeoDirectory")]
        [Category("Exception")]
        public void Importer_GeoDirectoryExceptions()
        {

        }

        [Test]
        [Category("Importer")]
        [Category("Daft")]
        [Category("Exception")]
        public void Importer_DaftExceptions()
        {

        }

        [Test]
        [Category("Importer")]
        [Category("ErrorHandling")]
        public void Importer_ServiceUnavailableError()
        {

        }

        [Test]
        [Category("Importer")]
        [Category("ErrorHandling")]
        public void Importer_DBUnavailable()
        {

        }

        [Test]
        [Category("Importer")]
        [Category("ErrorHandling")]
        public void Importer_Security_HeaderMissing()
        {

        }

        [Test]
        [Category("Importer")]
        [Category("ErrorHandling")]
        public void Importer_SpaceUnavailable()
        {

        }
    }


    [TestFixture]
    public class ConvertorTest
    {
        [Test]
        [Category("Convertor")]
        [Category("GeoDirectory")]
        public void Convertor_GeoDirectory()
        {

        }

        [Test]
        [Category("Convertor")]
        [Category("Daft")]
        public void Convertor_Daft()
        {

        }

        [Test]
        [Category("Convertor")]
        [Category("MyHome")]
        public void Convertor_MyHome()
        {

        }

        [Test]
        [Category("Convertor")]
        [Category("GeoDirectory")]
        [Category("Exceptions")]
        public void Convertor_ExceptionInvalidFieldMappings()
        {

        }

        [Test]
        [Category("Convertor")]
        [Category("GeoDirectory")]
        [Category("Exceptions")]
        public void Convertor_ExceptionNoDataAvailable()
        {

        }

        [Test]
        [Category("Convertor")]
        [Category("GeoDirectory")]
        [Category("Exceptions")]
        public void Convertor_ExceptionX1()
        {

        }

        [Test]
        [Category("Convertor")]
        [Category("GeoDirectory")]
        [Category("Exceptions")]
        public void Convertor_ExceptionX2()
        {

        }


    }

    [TestFixture]
    public class ServiceTest
    {
        [Test]
        [Category("Service")]
        [Category("GeoDirectory")]
        public void Convertor_GeoDirectory()
        {

        }

        [Test]
        [Category("Service")]
        [Category("Daft")]
        public void Convertor_Daft()
        {

        }

        [Test]
        [Category("Service")]
        [Category("MyHome")]
        public void Convertor_MyHome()
        {

        }
    }

    [TestFixture]
    public class ConsoleTest
    {
        [Test]
        [Category("Console")]
        [Category("GeoDirectory")]
        public void Console_StartService()
        {

        }

        [Test]
        [Category("Console")]
        [Category("Security")]
        public void Console_StartServiceCorrectCreditentials()
        {

        }

        [Test]
        [Category("Console")]
        [Category("Security")]
        public void Console_StartServiceIncorrectCreditentials()
        {

        }
    }
}

