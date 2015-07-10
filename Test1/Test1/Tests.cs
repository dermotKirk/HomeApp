using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Core;
using NUnit.Util.Extensibility;

using Microsoft;

using System;
using System.IO;
using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Formatting;
using AIB.JSON.Handlers;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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



    /// <summary>
    /// Check and report multiple conditions in unit tests.
    /// Build up the assertions required, then iteriate over them, catch just the message without a stack trace and display
    /// outputs the number of failures and the indiviual asset failures
    /// Adapted from: http://blog.drorhelper.com/2011/02/multiple-asserts-done-right.html
    /// </summary>
    public class AssertAll
    {
        public static void Execute(params Action[] assertionsToRun)
        {
            var errorMessages = new List<String>();
            foreach (var action in assertionsToRun)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception exc)
                {
                    errorMessages.Add(exc.Message);
                }
            }

            if (errorMessages.Count <= 0)
                return;

            var errorText = new StringBuilder();
            foreach (string e in errorMessages)
            {
                if (errorText.Length > 0)
                    errorText.Append(Environment.NewLine);
                //errorText.Append(digestStackTrace(e));
                errorText.Append(e);
            }

            Assert.Fail(string.Format("{0}/{1} conditions failed:{2}{2}{3}", errorMessages.Count, assertionsToRun.Length,
                                      Environment.NewLine, errorText));
        }
    }


    public class RestClient
    {
        private HttpClient _client = null;

        public HttpClient client(string uri)
        {
            // Write action to console
            //Console.WriteLine("No active HttpClient, creating a new one.");

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
    [Category("API")]
    [Category("Looking")]
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
        [Category("API")]
        [Category("Looking")]
        [Category("PropertyListingUsingLatLongRadius")]
        [Category("404HTTPExceptionsExceptions")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/exxxxx/-9.9150/1000", "404", TestName = "2,1 - Letters used for Lat" )]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-exxxxx/1000", "404", TestName = "2,2 - Letters used for Long")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-9.9150/exxxxx", "404", TestName = "2,3 - Letters used for Radius")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/£$^/-9.9150/1000", "404", TestName = "2,4 - Symbols used for Lat")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-£$^/1000", "404", TestName = "2,5  - Symbols used for Long")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-9.9150/£$^", "404", TestName = "2,6 - Symbols used for Radius")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/100000000000000000/details", "404", TestName = "4,5 - Super large non existent property id of 100000000000000000")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/-1/details", "404", TestName = "4,3 - Minus 1 in the Property Id call")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/0/details", "404", TestName = "4,4 - 0 value for the Property Id call")]
        public void API_PropertyListingHTTP404ErrorExceptions(string strURL, string HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);            
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound, "Expected HTTP Code 404 Not Found but instead found " + httpResponse.StatusCode);
        }
        [Test]
        [Category("API")]
        [Category("Looking")]
        [Category("PropertyListingUsingLatLongRadius")]
        [Category("400HTTPExceptions")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/153.9311/-9.9150/1000", "400", TestName = "2,7 - Numbers outside useable range for Lat")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-199.9150/1000", "400", TestName = "2,8 - Numbers outside useable range for Long")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-199.9150/1000", "400", TestName = "2,8 - Numbers outside useable range for Long")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-9.9150/0", "400", TestName = "2,10 - Numbers outside useable range for Radius")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-9.9150/-1", "400", TestName = "2,11 - Radius is invalid minus number")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/0/0/0", "400", TestName = "2,18 - Zero Geo Cords passed with 0 radius")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-199.9150/1000*", "400", TestName = "2,29 - URL contains *")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/100000000000000000/details*", "400", TestName = "4,6 - URI has a trailing asterix *")]
        public void API_PropertyListingHTTP400ErrorExceptions(string strURL, string HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "Expected HTTP Code 400 Bad Request but instead found " + httpResponse.StatusCode);
        }

        [Test]
        [Category("API")]
        [Category("Looking")]
        [Category("PropertyListingUsingSummaryListingFiltered")]
        [Category("200HTTPSuccess")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/summary?filterCounties=Dublin", "200", TestName = "1,1 - Summary Prop Listing within Dublin grouped to Locality")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/summary?filterCounties=Kerry", "200", TestName = "1,3 - Summary Prop Listing within Kerry grouped to Locality")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/summary?filterCounties=Galway", "200", TestName = "1,6 - Summary Prop Listing within Galway grouped to Locality")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/summary?filterCounties=Leitrim", "200", TestName = "1,9 - Summary Prop Listing within Leitrim grouped to Locality")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-9.9150/300000", "200", TestName = "2,10 - Radius outside useable Radiusmassive result set < 101")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/65/-18/1000", "200", TestName = "2,12 - GeoCord outside Ireland > Iceland empty results ")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/55.950000/-3.183333/1000", "200", TestName = "2,13 - GeoCord outside Ireland > Scotland Edinburgh empty results ")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/48.8534100/2.3488000/1000", "200", TestName = "2,14 - GeoCord outside Ireland > Paris France empty results ")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/62.000000/10.083333/1000", "200", TestName = "2,15 - GeoCord outside Ireland > Norway empty results ")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/0/0/1000", "200", TestName = "2,17 - Zero Geo Cords passed with a valid radius should return nothing in set")]

        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-9.9150", "404", TestName = "2,19 - No radius supplied blank and missing trailing slash from url")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-9.9150/0", "200", TestName = "2,20 - Zero radius supplied - optional value used if not passed")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9311/-9.9150//", "200", TestName = "2,21 - Blank radius passed with a trailing / - means passing no radius")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.3330600/-6.2488900/10000/", "200", TestName = "2,22 - Search Dublin with massive radius 10000M - results limited to 100 by overall count returned.")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.3330600/-6.2488900/100/", "200", TestName = "2,23 - Search Dublin with small radius 100M - results limited to 100 but overall count returned small.")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.3330600/-6.2488900/250/", "200", TestName = "2,24 - Search Dublin with medium radius 250M")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.3330600/-6.2488900/50/", "200", TestName = "2,25 - Zero Geo Cords passed with a valid very small radius 50M should return nothing in set")]


        public void API_PropertySummarySuccess(string strURL, string HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode);
        }

        [Test]
        [Category("API")]
        [Category("Looking")]
        [Category("PropertyListingUsingSummaryListingFiltered")]
        [Category("200HTTPSuccess")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/summary?filterCounties=LXXXeitrim", "200", TestName = "2,26 - Summary Prop Listing within county LXXXeitrim returning empty result set")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/summary?filterCounties=!£$^", "200", TestName = "2,27 - Summary of Property Listing by Invalid County Name using symbols returning empty result set")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/summary?filterCounties=]", "200", TestName = "2,28  - Summary of Property Listing by Invalid County Name using blank returning complete result set of ALL COUNTIES and ALL Localities")]
        public void API_PropertySummaryInvalidPropertyAPICalls(string strURL, string HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode);
        }

        [Test]
        [Category("API"), Category("Looking"), Category("CountyPropertyListingLocalFiltered")]
        [Category("200HTTPSuccess")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/Dublin?locality=Dublin 1", "200", TestName = "1,2 - Prop Listing within county Dublin filtered by Dublin 1 locality returning large result set < 100")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/Kerry", "200", TestName = "1,4 - Prop Listing within county Kerry not filtered by locality returning large result set < 100")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/Galway?locality=Achill", "200", TestName = "1,7 - Prop Listing within county Galway filtered by Achill Island locality returning small result set or none")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/Leitrim?locality=", "200", TestName = "1,8 - Prop Listing within county Leitrim filtered by empty locality returning ALL Leitrim properties")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9908/-8.0636/10000", "200", TestName = "1,10 - Property Listing within 10000 meters of Leitrim(possibly none)")]

        public void API_CountyPropertyListingsLocalityFilteredAPICalls(string strURL, string HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode);
        }

        [Test]
        [Category("API"), Category("Bootstrap"), Category("200HTTPSuccess")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/system/bootstrap", "200", TestName = "3,1 - Standard Boostrap API Call")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/system/bootstrap?asdasd=asdasda", "200", TestName = "3,2 - Bootstrap call with additional patameter tagged on")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/Galway?locality=Achill", "200", TestName = "1,7 - Prop Listing within county Galway filtered by Achill Island locality returning small result set or none")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/Leitrim?locality=", "200", TestName = "1,8 - Prop Listing within county Leitrim filtered by empty locality returning ALL Leitrim properties")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/53.9908/-8.0636/10000", "200", TestName = "1,10 - Property Listing within 10000 meters of Leitrim(possibly none)")]

        public void API_BootstrapAPICalls(string strURL, string HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode);
        }

        [Test]
        [Category("API"), Category("PropertyDetails"), Category("200HTTPSuccess")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property/100/details", "200", TestName = "4,1 - Valid Property Call")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/property//details", "200", TestName = "4,2 - Missing Property Id in the API call")]

        public void API_PropertyDetailsAPICalls(string strURL, string HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode);
        }

        [Test]
        [Category("API"), Category("Amenity"), Category("200HTTPSuccess")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/amenity/53.9908/-8.0636/10000", "200", TestName = "5,1,1 - Achill with a large radius")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/amenity/53.350140/-6.266155/250", "200", TestName = "5,1,2 - Dublin with a small radius")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/looking/amenity/53.9311/-9.9150/1000", "200", TestName = "5,1,3 - Leitrim with medium radius")]

        public void API_AmenityAPISuccessCalls(string strURL, string HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode);
        }

        [Test]
        [Category("API"), Category("Amenity"), Category("400HTTPBadRequest")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/!£$/&%^/~@~@", "400", TestName = "5,6,3 - Amenity API call with invalid symbols parameters - HTTP 404 returned")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/48.8534100/2.3488000/1000*", "400", TestName = "5,6,11 - Amenity API call with outside of Ireland boundary (Paris, France) parameters - with trailing *")]
        public void API_AmenityAPIBadRequestCalls(string strURL, string HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "Expected HTTP Code 400 Not Found but instead found " + httpResponse.StatusCode);
        }

        [Test]
        [Category("API"), Category("Amenity"), Category("404HTTPNotFound")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/53.350140/-6.266155", 404, TestName = "5,2 - Dublin with no Radius returns 404")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity//-6.266155/250", 404, TestName = "5,3 - Amenity call missing lat parameter - should return HTTP 404")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/53.350140//250", 404, TestName = "5,4 - Amenity call missing long parameter - should return HTTP 404")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity///", 404, TestName = "5,5 - Amenity call missing all parameters - should return HTTP 404")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity", 404, TestName = "5,6,1 - Amenity API call with no parameters missing trailing slash HTTP 404 returned")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/", 404, TestName = "5,6,2 - Amenity API call with no parameters including trailing slash HTTP 404 returned")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/aaaxxx/axxxxaa/qqqq", "404", TestName = "5,6,4 - Amenity API call with invalid characters parameters - HTTP 404 returned.")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/0/0/0", 404, TestName = "5,6,5 - Amenity API call with invalid out of bounds ZERO values parameters - HTTP 404 returned.")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/-1/-1/-1", 404, TestName = "5,6,6 - Amenity API call with invalid out of bounds negative values parameters - HTTP 404 returned.")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/62.000000/10.083333/250", 404, TestName = "5,6,7 - Amenity API call with outside of Ireland boundary (Norway) parameters HTTP 404 returned.")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/48.8534100/2.3488000/1000", 404, TestName = "5,6,8 - Amenity API call with outside of Ireland boundary (Paris, France) parameters HTTP 404 returned.")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/768768/979879887/098098098", 404, TestName = "5,6,9 - Amenity API call with massive boundary parameters HTTP 404 returned.")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/amenity/???/???/???", 404, TestName = "5,6,10 - Amenity API call with question marks as parameters HTTP 404 returned.")]
        public void API_AmenityAPINotFoundCalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.GetAsync(strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound, "Expected HTTP Code 404 Not Found but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.NotFound, "Test Data setup for a 404 check but executed result produced a " + HTTPStatusCode + " HTTP code.  Check test data is correct or API call has changed")
                );
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
    public class API_BudgetStress
    {

        [Test]
        [Category("API"), Category("BudgetStress"), Category("200HTTPOK")]
        [TestCase("budget/stress", "200", 150000, 10000, 400, 1500, 35, true, 795.200000, 0.0568, TestName = "1,1 - Preparer makes call to Stress Payment budget wizard API")]
        [TestCase("budget/stress", "200", 250000, 20000, 2000, 2000, 32, true, 1343.200000, 0.05840, TestName = "1,2 - Looking makes call to Stress Payment budget wizard API")]
        [TestCase("budget/stress", "200", 350000, 25000, 1600, 1800, 35, true, 1846.000000, 0.0568, TestName = "1,3 - Applying makes call to Stress Payment budget wizard API")]
        [TestCase("budget/stress", "200", 285000, 60000, 2200, 2400, 35, true, 1278.000000, 0.0568, TestName = "1,4 - Mover makes call to Stress Payment budget wizard API")]
        [TestCase("budget/stress", "200", 650000, 120000, 1000, 500, 15, false, 4462.600000, 0.08420, TestName = "1,5 - Upgrader call to savings budget wizard API")]
        [TestCase("budget/stress", "200", 750000, 150000, 3000, 2200, 12, false, 5844.00000, 0.0974, TestName = "1,6 - Max limit 750k makes call to savings budget wizard API")]
        [TestCase("budget/stress", "200", 770000, 195000, 1000, 1750, 22, false, 3915.750000, 0.06810, TestName = "1,7 - Over Max limit makes call 770k Make call to savings budget wizard API")]
        [TestCase("budget/stress", "200", 850000, 600000, 8000, 3000, 5, true, 4830.00000, 0.1932, TestName = "1,8 - Retiree Millionaire Upgrader moving in 18 months makes call to savings budget wizard API")]
        [TestCase("budget/stress", "200", 756000, 300000, 2300, 3300, 7, false, 6653.04000, 0.1459, TestName = "1,9 - 756k Edge case makes call to savings budget wizard API")]

        public void API_BudgetStressAPISuccessCalls(string strURL, string HTTPStatusCode, float Price, float Deposit, float MonthlySavings, float MonthlyRent, int MortgageTerm, bool PassedStressTest, decimal StressAmount, decimal StressRate)
        {
            var testData = new StressBudget();
            testData.Price = Price;
            testData.Deposit = Deposit;
            testData.MonthlySavings = MonthlySavings;
            testData.MonthlyRent = MonthlyRent;
            testData.MortgageTerm = MortgageTerm;

            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.PostAsJsonAsync<StressBudget>("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL, testData);
            HttpResponseMessage httpResponse = rClient.Result;
            
            var sTest = httpResponse.Content.ReadAsAsync<BudgetStressResponse>();
            var testDataResult = sTest.Result;

            AssertAll.Execute(
            () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
            () => Assert.IsTrue(testDataResult.PassedStressTest == PassedStressTest, "Expected PassedStressTest value of " + PassedStressTest + " but found " + testDataResult.PassedStressTest),
            () => Assert.IsTrue(testDataResult.StressAmount == StressAmount, "Expected Stress Amount to be " + StressAmount + " but found " + testDataResult.StressAmount),
            () => Assert.IsTrue(testDataResult.StressRate == StressRate, "Expected Stress Rate Amount of " + StressRate + " but found " + testDataResult.StressRate)
            );
        }

        [Test]
        [Category("API"), Category("BudgetStress"), Category("400HTTPBadRequest")]
        [TestCase("budget/stress", "400", 74999, 0, 100, 200, 35, "[ArgumentOutOfRangeException] The requested Mortgage amount is outside of the acceptable range", "Parameter name: Mortgage", "", TestName = "2,1 - Mortgage Below Min threshold value, 400 error thrown and error message found")]
        [TestCase("budget/stress", "400", 750001, 0, 100, 200, 35, "[ArgumentOutOfRangeException] The requested Mortgage amount is outside of the acceptable range", "Parameter name: Mortgage", "", TestName = "2,2 - Mortgage Above Max threshold value")]
        [TestCase("budget/stress", "400", 75000, 0, 100, 200, 36, "[ArgumentOutOfRangeException] The requested Mortgage Term is outside of the acceptable range", "Parameter name: MortgageTerm", "", TestName = "2,3 - Mortgage Term Below Min value")]
        [TestCase("budget/stress", "400", 75000, 0, 100, 200, 3, "[ArgumentOutOfRangeException] The requested Mortgage Term is outside of the acceptable range", "Parameter name: MortgageTerm", "", TestName = "2,4 - Mortgage Term Above Max value")]
        //below will fail for now until I catch all errors

        [TestCase("budget/stress", "400", 75000, 0, 100, 200, 35, "[ArgumentOutOfRangeException] The requested Mortgage amount is outside of the acceptable range", "Parameter name: Mortgage", "", TestName = "2,5 - Mortgage Repayment Stress Test Fails as Savings and Amounts Less that Expected Stressed Payment")]
        [TestCase("budget/stress", "400", -750001, 0, 100, 200, 35, "[ArgumentOutOfRangeException] The requested Mortgage amount is outside of the acceptable range", "Parameter name: Mortgage", "", TestName = "2,6 - House Price Amount minus value")]
        [TestCase("budget/stress", "400", "XXXXXX", 300000, 2300, 3300, 7, false, 6653.04000, 0.1459, TestName = "2,7 - House Price Amount is character value")]
        [TestCase("budget/stress", "400", "£$^&", 300000, 2300, 3300, 7, false, 6653.04000, 0.1459, TestName = "2,8 - House Price Amount is symbol value")]
        [TestCase("budget/stress", "400", 756000, -300000, 2300, 3300, 7, false, 6653.04000, 0.1459, TestName = "2,9 - Deposit Amount minus value")]
        [TestCase("budget/stress", "400", 756000, "XXXXXX", 2300, 3300, 7, false, 6653.04000, 0.1459, TestName = "2,10 - Deposit Amount character value")]
        [TestCase("budget/stress", "400", 756000, "!£$^", 2300, 3300, 7, false, 6653.04000, 0.1459, TestName = "2,11 - Deposit Amount symbol value")]
        public void API_BudgetStressAPIBadRequestCalls(string strURL, string HTTPStatusCode, float Price, float Deposit, float MonthlySavings, float MonthlyRent, int MortgageTerm, string ContainsErrorMsg1, string ContainsErrorMsg2, string ContainsErrorMsg3)
        {
            var testData = new StressBudget();
            testData.Price = Price;
            testData.Deposit = Deposit;
            testData.MonthlySavings = MonthlySavings;
            testData.MonthlyRent = MonthlyRent;
            testData.MortgageTerm = MortgageTerm;

            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.PostAsJsonAsync<StressBudget>("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL, testData);
            HttpResponseMessage httpResponse = rClient.Result;

            var sTest = httpResponse.Content.ReadAsStringAsync();
            
            var testDataResult = sTest.Result;
            AssertAll.Execute(
            () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "Expected HTTP Code 400 BadRequest but instead found " + httpResponse.StatusCode),
            () => Assert.IsTrue(testDataResult.ToString().Contains(ContainsErrorMsg1) == true, "Expected Error Message Containing value of " + ContainsErrorMsg1 + " but error found instead is " + testDataResult),
            () => Assert.IsTrue(testDataResult.ToString().Contains(ContainsErrorMsg2) == true, "Expected Error Message Containing value of " + ContainsErrorMsg2 + " but error found instead is " + testDataResult)
//            () => Assert.IsTrue(testDataResult.StressRate == StressRate, "Expected Stress Rate Amount of " + StressRate + " but found " + testDataResult.StressRate)
            );
        }


    }

    [TestFixture]
    public class API_BudgetSaving
    {

        [Test]
        [Category("API"), Category("BudgetSavings"), Category("200HTTPOK")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, true, 150000, 0, 15000, "416.66666666666666666666666667", TestName = "1,1 - Preparer makes call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, true, 250000, 0, 28000, "777.77777777777777777777777778", TestName = "1,2 - Looking makes call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 350000, 0, 70000, "1944.4444444444444444444444444", TestName = "1,3 - Applying makes call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 285000, 60000, 0, "0", TestName = "1,4 - Mover makes call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 24, false, 650000, 10000, 120000, "5000", TestName = "1,5 - Upgrader call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 12, false, 750000, 0, 150000, "12500", TestName = "1,6 - Max limit 750k makes call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 24, false, 770000, 195000, 0, "0", TestName = "1,7 - Over Max limit makes call 770k Make call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 18, false, 850000, 600000, 0, "0", TestName = "1,8 - Retiree Millionaire Upgrader moving in 18 months makes call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 756000, 300000, 0, "0", TestName = "1,9 - 756k Edge case makes call to savings budget wizard API")]

        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, -756000, 300000, 0, "0", TestName = "2,1 - Pass minus property price call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, 1, 756000, 300000, 0, "0", TestName = "2,2 - Pass 1 First Buyer (not true / false) call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, -1, 756000, 300000, 0, "0", TestName = "2,3 - Pass minus 1 First Buyer (not true / false) call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 756000, -300000, 0, "0", TestName = "2,4 - Pass minus Gift Amount to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", -36, false, 756000, 300000, 0, "0", TestName = "2,5 - Pass minus Duration to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 0, 300000, 0, "0", TestName = "2,6 - Pass 0 property price to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, -2, 756000, 300000, 0, "0", TestName = "2,7 - Pass minus 2 First Buyer value to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 0, false, 756000, 300000, 0, "0", TestName = "2,8 - Pass 0 Duration to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 75000, 0, 0, "0", TestName = "2,9 - Pass min threshold (75k) property price to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 750000, 0, 0, "0", TestName = "2,10 - 756k Pass max threshold (750k) property price to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 37, false, 750000, 0, 0, "0", TestName = "2,11 - Pass 37 Duration to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 750000, 0, 0, "0", TestName = "2,12 - Pass 99999 Duration to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 750000, 0, 0, "0", TestName = "2,13 - Pass AAAA Duration to call to savings budget wizard API")]
        [TestCase("http://homeappapihost-dev.elasticbeanstalk.com/budget/saving", "200", 36, false, 750000, 0, 0, "0", TestName = "2,14 -  Pass £$^& Duration to call to savings budget wizard API")]
        public void API_SavingsAPINotFoundCalls(string strURL, string HTTPStatusCode, int Duration, bool BuyerType, decimal TargetPurchasePrice, decimal GiftAmount, decimal TargetDeposit, string TargetSavings)
        {
            var testData = new SavingsBudget();
            testData.Duration = Duration;
            testData.FirstTimeBuyer = BuyerType;
            testData.TargetPurchasePrice = TargetPurchasePrice;
            testData.GiftAmount = GiftAmount;

            RestClient API_client = new RestClient();
            var client = API_client.client(strURL);
            // is the server available??            
            var rClient = client.PostAsJsonAsync<SavingsBudget>(strURL, testData);
            HttpResponseMessage httpResponse = rClient.Result;
            var sTest = httpResponse.Content.ReadAsAsync<SavingsBudget>();
            var testDataResult = sTest.Result;

            AssertAll.Execute(
            () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
            () => Assert.IsTrue(testDataResult.Duration == testData.Duration, "Expected Duration of " + Duration + " but found " + testDataResult.Duration),
            () => Assert.IsTrue(testDataResult.FirstTimeBuyer == testData.FirstTimeBuyer, "Expected First time Buyer to be " + BuyerType + " but found " + testDataResult.FirstTimeBuyer),
            () => Assert.IsTrue(testDataResult.GiftAmount == testData.GiftAmount, "Expected Gift Amount of " + GiftAmount + " but found" + testDataResult.GiftAmount),
            () => Assert.IsTrue(testDataResult.TargetDeposit == TargetDeposit, "Expected TargetDeposit Amount of " + TargetDeposit + " but found " + testDataResult.TargetDeposit),
            () => Assert.IsTrue(testDataResult.TargetPurchasePrice == testData.TargetPurchasePrice, "Expected TargetPurchasePrice of " + TargetPurchasePrice + "but found " + testDataResult.TargetPurchasePrice),
            () => Assert.IsTrue(testDataResult.TargetSavings == Convert.ToDecimal(TargetSavings), "Expected TargetSavings Amount of " + TargetSavings + " but found " + testDataResult.TargetSavings)
            );
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

