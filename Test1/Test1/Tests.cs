using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Core;
using NUnit.Util.Extensibility;

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
        [TestCase("looking/property/exxxxx/-9.9150/1000", 404, TestName = "2,1 - Letters used for Lat")]
        [TestCase("looking/property/53.9311/-exxxxx/1000", 404, TestName = "2,2 - Letters used for Long")]
        [TestCase("looking/property/53.9311/-9.9150/exxxxx", 404, TestName = "2,3 - Letters used for Radius")]
        [TestCase("looking/property/£$^/-9.9150/1000", 404, TestName = "2,4 - Symbols used for Lat")]
        [TestCase("looking/property/53.9311/-£$^/1000", 404, TestName = "2,5  - Symbols used for Long")]
        [TestCase("looking/property/53.9311/-9.9150/£$^", 404, TestName = "2,6 - Symbols used for Radius")]
        [TestCase("looking/property/100000000000000000/details", 404, TestName = "4,5 - Super large non existent property id of 100000000000000000")]
        [TestCase("looking/property/-1/details", 404, TestName = "4,3 - Minus 1 in the Property Id call")]
        [TestCase("looking/property/0/details", 404, TestName = "4,4 - 0 value for the Property Id call")]
        [TestCase("looking/property/53.9311/-9.9150", 404, TestName = "2,19 - No radius supplied blank and missing trailing slash from url")]
        public void API_PropertyListingHTTP404ErrorExceptions(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound, "Expected HTTP Code 404 OK but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.NotFound, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
            );
        }
        [Test]
        [Category("API")]
        [Category("Looking")]
        [Category("PropertyListingUsingLatLongRadius")]
        [Category("400HTTPExceptions")]
        [TestCase("looking/property/153.9311/-9.9150/1000", 400, TestName = "2,7 - Numbers outside usable range for Lat")]
        [TestCase("looking/property/53.9311/-199.9150/1000", 400, TestName = "2,8 - Numbers outside usable range for Long")]
        [TestCase("looking/property/53.9311/-9.9150/0", 400, TestName = "2,10 - Numbers outside usable range for Radius")]
        [TestCase("looking/property/53.9311/-9.9150/-1", 400, TestName = "2,11 - Radius is invalid minus number")]
        [TestCase("looking/property/0/0/0", 400, TestName = "2,18 - Zero Geo Cords passed with 0 radius")]
        [TestCase("looking/property/53.9311/-9.9150/0", 400, TestName = "2,20a - Achill Island - Zero radius supplied - optional value used if not passed")]
        [TestCase("looking/property/53.3330600/-6.2488900/0", 400, TestName = "2,20b - Dublin City - Zero radius supplied - optional value used if not passed")]
        [TestCase("looking/property/53.9311/-199.9150/1000*", 400, TestName = "2,29 - URL contains *")]
        [TestCase("looking/property/100000000000000000/details*", 400, TestName = "4,6 - URI has a trailing astrix *")]
        public void API_PropertyListingHTTP400ErrorExceptions(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "Expected HTTP Code 400 OK but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.BadRequest, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
            );
        }

        [Test]
        [Category("API")]
        [Category("Looking")]
        [Category("PropertyListingUsingSummaryListingFiltered")]
        [Category("200HTTPSuccess")]
        [TestCase("looking/property/summary?filterCounties=Dublin", 200, TestName = "1,1 - Summary Prop Listing within Dublin grouped to Locality")]
        [TestCase("looking/property/summary?filterCounties=Kerry", 200, TestName = "1,3 - Summary Prop Listing within Kerry grouped to Locality")]
        [TestCase("looking/property/summary?filterCounties=Galway", 200, TestName = "1,6 - Summary Prop Listing within Galway grouped to Locality")]
        [TestCase("looking/property/summary?filterCounties=Leitrim", 200, TestName = "1,9 - Summary Prop Listing within Leitrim grouped to Locality")]
        [TestCase("looking/property/53.9311/-9.9150/300000", 200, TestName = "2,10 - Radius outside usable Radius massive result set < 101")]
        [TestCase("looking/property/65/-18/1000", 200, TestName = "2,12 - GeoCord outside Ireland > Iceland empty results ")]
        [TestCase("looking/property/55.950000/-3.183333/1000", 200, TestName = "2,13 - GeoCord outside Ireland > Scotland Edinburgh empty results ")]
        [TestCase("looking/property/48.8534100/2.3488000/1000", 200, TestName = "2,14 - GeoCord outside Ireland > Paris France empty results ")]
        [TestCase("looking/property/62.000000/10.083333/1000", 200, TestName = "2,15 - GeoCord outside Ireland > Norway empty results ")]
        [TestCase("looking/property/0/0/1000", 200, TestName = "2,17 - Zero Geo Cords passed with a valid radius should return nothing in set")]

        [TestCase("looking/property/53.9311/-9.9150//", 200, TestName = "2,21 - Blank radius passed with a trailing / - means passing no radius")]
        [TestCase("looking/property/53.3330600/-6.2488900/10000/", 200, TestName = "2,22 - Search Dublin with massive radius 10000M - results limited to 100 by overall count returned.")]
        [TestCase("looking/property/53.3330600/-6.2488900/100/", 200, TestName = "2,23 - Search Dublin with small radius 100M - results limited to 100 but overall count returned small.")]
        [TestCase("looking/property/53.3330600/-6.2488900/250/", 200, TestName = "2,24 - Search Dublin with medium radius 250M")]
        [TestCase("looking/property/53.3330600/-6.2488900/50/", 200, TestName = "2,25 - Zero Geo Cords passed with a valid very small radius 50M should return nothing in set")]


        public void API_PropertySummarySuccess(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
            );
        }

        [Test]
        [Category("API")]
        [Category("Looking")]
        [Category("PropertyListingUsingSummaryListingFiltered")]
        [Category("200HTTPSuccess")]
        [TestCase("looking/property/summary?filterCounties=LXXXeitrim", 200, TestName = "2,26 - Summary Prop Listing within county LXXXeitrim returning empty result set")]
        [TestCase("looking/property/summary?filterCounties=!£$^", 200, TestName = "2,27 - Summary of Property Listing by Invalid County Name using symbols returning empty result set")]
        [TestCase("looking/property/summary?filterCounties=]", 200, TestName = "2,28  - Summary of Property Listing by Invalid County Name using blank returning complete result set of ALL COUNTIES and ALL Localities")]
        public void API_PropertySummaryInvalidPropertyAPICalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
            );
        }

        [Test]
        [Category("API"), Category("Looking"), Category("CountyPropertyListingLocalFiltered")]
        [Category("200HTTPSuccess")]
        [TestCase("looking/property/Dublin?locality=Dublin 1", 200, TestName = "1,2 - Prop Listing within county Dublin filtered by Dublin 1 locality returning large result set < 100")]
        [TestCase("looking/property/Kerry", 200, TestName = "1,4 - Prop Listing within county Kerry not filtered by locality returning large result set < 100")]
        [TestCase("looking/property/Galway?locality=Achill", 200, TestName = "1,7 - Prop Listing within county Galway filtered by Achill Island locality returning small result set or none")]
        [TestCase("looking/property/Leitrim?locality=", 200, TestName = "1,8 - Prop Listing within county Leitrim filtered by empty locality returning ALL Leitrim properties")]
        [TestCase("looking/property/53.9908/-8.0636/10000", 200, TestName = "1,10 - Property Listing within 10000 meters of Leitrim(possibly none)")]

        public void API_CountyPropertyListingsLocalityFilteredAPICalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
            );
        }

        [Test]
        [Category("API"), Category("Bootstrap"), Category("200HTTPSuccess")]
        [TestCase("system/bootstrap", 200, TestName = "3,1 - Standard Boostrap API Call")]
        [TestCase("system/bootstrap?asdasd=asdasda", 200, TestName = "3,2 - Bootstrap call with additional patameter tagged on")]

        public void API_BootstrapAPICalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
                );
        }

        [Test]
        [Category("API"), Category("PropertyDetails"), Category("200HTTPSuccess")]
        [TestCase("looking/property//details", 200, TestName = "4,2 - Missing Property Id in the API call")]

        public void API_PropertyDetailsAPICalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
                );
        }

        [Test]
        [Category("API"), Category("PropertyDetails"), Category("200HTTPSuccess")]
        [TestCase("looking/property/Dublin?locality=Dublin", 200, TestName = "4,1 - Valid Property Call")]

        public void API_PropertyDetailsFromAPICallsSearches(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            var propertyUrl = "";
            PropertyDetails pdPropertyDetails = null;
            var PropertySearchResultAreaWrapper = httpResponse.Content.ReadAsAsync<PropertySearchResultAreaWrapper>();
            if (PropertySearchResultAreaWrapper.Result.SearchResults.Count() > 1)
            {
                Console.WriteLine("Found " + PropertySearchResultAreaWrapper.Result.SearchResults.Count() + " properties from the search");
                Console.WriteLine("Attempting to retrieve the first property from link " + PropertySearchResultAreaWrapper.Result.SearchResults[0].InfoLink.ToString());
                propertyUrl = PropertySearchResultAreaWrapper.Result.SearchResults[0].InfoLink.ToString();
                rClient = client.GetAsync(propertyUrl);
                httpResponse = rClient.Result;
                pdPropertyDetails = httpResponse.Content.ReadAsAsync<PropertyDetails>().Result;
                Console.WriteLine("Retrieved the first property with Id of " + pdPropertyDetails.Id + " at the following address " + pdPropertyDetails.FullAddress);
                AssertAll.Execute(
                    () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                    () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed"),
                    () => Assert.IsTrue(pdPropertyDetails.Id != null, "Id was null but expected a value.  ID was" + pdPropertyDetails.Id),
                    () => Assert.IsTrue(pdPropertyDetails.Type == "PropertyDetail", "Expected PropertyDetails.Type == \"PropertyDetail\" but found it was equal to " + pdPropertyDetails.Type + ".  Property Detail type has changed within the API."),
                    () => Assert.IsTrue(pdPropertyDetails.County == "Co. Dublin", "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed"),
                    () => Assert.IsTrue(PropertySearchResultAreaWrapper.Result.SearchResults.Count() > 0, "Test Data expected greater than 0 amount of records counted but found " + PropertySearchResultAreaWrapper.Result.SearchResults.Count() + " Properties count.  Check test data is correct or API call has changed, or possibly there have are not ANY properties.")
                    );
            }
            else
            {
                Console.WriteLine("There were no properties returned Count is " + PropertySearchResultAreaWrapper.Result.SearchResults.Count() + " check the views are deployed and the API is up and available.");
                AssertAll.Execute(
                    () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                    () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
                    );
            }
        }

        [Test]
        [Category("API"), Category("AmenitiesSearch"), Category("200HTTPSuccess")]
        [TestCase("looking/amenity/51.9215752605773/-8.45968250977375/?filterBy=Education", 200, TestName = "5,7 - Amenities Search of schools, followed by a Valid Property Search in area of Amenity with default radius")]

        public void API_PropertyDetailsFromAmenitiesAPISearches(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            PropertySearchResultAreaWrapper psrSearchResults = null;
            var AmenitiesResultEnvelope = httpResponse.Content.ReadAsAsync<AmenitiesResultEnvelope>();
            if (AmenitiesResultEnvelope.Result.SearchResults.Count() > 1)
            {
                Console.WriteLine("Found " + AmenitiesResultEnvelope.Result.SearchResults.Count() + " amenities from the search");
                Console.WriteLine("Attempting to retrieve the first property from link " + AmenitiesResultEnvelope.Result.SearchResults[0].InfoLink.ToString());
                //as we passed in the exact cords for this school amenity, school should be the first one in the result set, we should have a range of 0,
                //  ALWAYS, grab value to be validated at the end of the test.  Display Name should also match the expected school name.
                var ZeroRangeValueExpected = AmenitiesResultEnvelope.Result.SearchResults[0].Range;
                var SchoolNameExpected = AmenitiesResultEnvelope.Result.SearchResults[0].DisplayName;
                // This is the school we want to perform our property searches around, we will default to 1000M radius
                // and this means that the last property returned should ALWAYS have a range of less than 1001M from the school.
                var propertySearchURL = "looking/property/" + AmenitiesResultEnvelope.Result.SearchResults[0].Latitude + "/" + AmenitiesResultEnvelope.Result.SearchResults[0].Longitude + "/";
                rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + propertySearchURL);
                httpResponse = rClient.Result;
                psrSearchResults = httpResponse.Content.ReadAsAsync<PropertySearchResultAreaWrapper>().Result;
                var propertyResultCount = psrSearchResults.SearchResults.Count();
                Console.WriteLine("Found " + psrSearchResults.SearchResults[propertyResultCount - 1].Range + " within 1000M of the school");
                AssertAll.Execute(
                    () => Assert.IsTrue(ZeroRangeValueExpected == 0, "Expected 0 range value as school cords were used but value of " + ZeroRangeValueExpected + " was found for the range value"),
                    () => Assert.IsTrue(SchoolNameExpected == "SCOIL OILIBHEÍR, DUBLIN HILL, CORK", "Expected Display Name to show SCOIL OILIBHEÍR, DUBLIN HILL, CORK but value of " + SchoolNameExpected + " was found for the Display Name value"),
                    () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                    () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed"),
                    () => Assert.IsTrue(psrSearchResults.SearchResults[propertyResultCount - 1].Range < 1001, "Range should be within default range of 100M.  Range was " + psrSearchResults.SearchResults[propertyResultCount - 1].Range),
                    () => Assert.IsTrue(AmenitiesResultEnvelope.Result.SearchResults.Count() > 0, "Test Data expected greater than 0 amount of records counted but found " + AmenitiesResultEnvelope.Result.SearchResults.Count() + " Properties count.  Check test data is correct or API call has changed, or possibly there have are not ANY properties.")
                    );
            }
            else
            {
                Console.WriteLine("There were no properties returned Count is " + AmenitiesResultEnvelope.Result.SearchResults.Count() + " check the views are deployed and the API is up and available.");
                AssertAll.Execute(
                    () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                    () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
                    );
            }
        }

        [Test]
        [Category("API"), Category("Amenity"), Category("200HTTPSuccess")]
        [TestCase("looking/amenity/53.9908/-8.0636/10000", 200, 100, "Leitrim", TestName = "5,1,1 - Leitrim with a large radius, limit to 100 results")]
        [TestCase("looking/amenity/53.350140/-6.266155/250", 200, 100, "Dublin", TestName = "5,1,2 - Dublin with a small radius")]
        [TestCase("looking/amenity/53.935068/-9.911023/1000", 200, 21, "Achill", TestName = "5,1,3 - Achill with medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Medical", 200, 10, "Near Achill", TestName = "5,1,4a - Near Achill with Medical filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Sports", 200, 1, "Near Achill", TestName = "5,1,4b - Near Achill with Sports filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Education", 200, 2, "Near Achill", TestName = "5,1,4c - Near Achill with Education filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Business", 200, 21, "Near Achill", TestName = "5,1,4d - Near Achill with Business filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Entertainment", 200, 4, "Near Achill", TestName = "5,1,4e - Near Achill with Entertainment filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Transport", 200, 1, "Near Achill", TestName = "5,1,4f - Near Achill with Transport filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Religious", 200, 2, "Near Achill", TestName = "5,1,4g - Near Achill with Religious filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Agriculture", 200, 0, "Near Achill", TestName = "5,1,4h - Near Achill with Agriculture filter, medium radius, no results set returned")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Industrial", 200, 0, "Near Achill", TestName = "5,1,4i - Near Achill with Industrial filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Communications", 200, 0, "Near Achill", TestName = "5,1,4j - Near Achill with Communications filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Food", 200, 3, "Near Achill", TestName = "5,1,4k - Near Achill with Medical Food, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Accommodation", 200, 4, "Near Achill", TestName = "5,1,4l - Near Achill with Accommodation filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Civil", 200, 5, "Near Achill", TestName = "5,1,4m - Near Achill with Civil filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Civil,Food,Sports", 200, 9, "Near Achill", TestName = "5,1,5a - Near Achill with Civil,Food,Sports filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Transport,Sports", 200, 2, "Near Achill", TestName = "5,1,5b - Near Achill with Transport,Sports filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=Religious,Education,Medical", 200, 14, "Near Achill", TestName = "5,1,5c - Near Achill with Religious,Education,Medical filter, medium radius")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=BadRequest,DKDKDKD", 200, 0, "Near Achill", TestName = "5,1,6,1 - Near Achill with invalid filter")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=!£$,DKDKDKD", 200, 0, "Near Achill", TestName = "5,1,6,2 - Near Achill with 2nd invalid symbol filter")]
        [TestCase("looking/amenity/53.9311/-9.9150/1000?filterBy=12212121,DKDKDKD", 200, 0, "Near Achill", TestName = "5,1,6,3 - Near Achill with 3rd invalid numbers filter")]

        public void API_AmenityAPISuccessCalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode, int AmenititesCount, string CoOrdNamedLocation)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            var AmenititesResultEnvelope = httpResponse.Content.ReadAsAsync<AmenitiesResultEnvelope>();
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code 200 OK but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.OK, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed"),
                () => Assert.IsTrue(AmenititesResultEnvelope.Result.SearchResults.Count() == AmenititesCount, "Test Data expected " + AmenititesCount + " count but found " + AmenititesResultEnvelope.Result.SearchResults.Count() + " amenities count.  Check test data is correct or API call has changed, or possibly there have been new amenities been added to Geo Directory.")
                );
            Console.WriteLine("Found " + AmenititesResultEnvelope.Result.SearchResults.Count() + " amenities around " + CoOrdNamedLocation + " with filtered call as seen in URL " + strURL);

        }

        [Test]
        [Category("API"), Category("Amenity"), Category("400HTTPBadRequest")]
        [TestCase("looking/amenity/!£$/&%^/~@~@", 400, TestName = "5,6,3 - Amenity API call with invalid symbols parameters - HTTP 404 returned")]
        [TestCase("looking/amenity/48.8534100/2.3488000/1000*", 400, TestName = "5,6,11 - Amenity API call with outside of Ireland boundary (Paris, France) parameters - with trailing *")]
        public void API_AmenityAPIBadRequestCalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "Expected HTTP Code 400 Not Found but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.BadRequest, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
                );
        }

        [Test]
        [Category("API"), Category("Amenity"), Category("404HTTPNotFound")]
        [TestCase("amenity/53.350140/-6.266155", 404, TestName = "5,2 - Dublin with no Radius returns 404")]
        [TestCase("amenity//-6.266155/250", 404, TestName = "5,3 - Amenity call missing lat parameter - should return HTTP 404")]
        [TestCase("amenity/53.350140//250", 404, TestName = "5,4 - Amenity call missing long parameter - should return HTTP 404")]
        [TestCase("amenity///", 404, TestName = "5,5 - Amenity call missing all parameters - should return HTTP 404")]
        [TestCase("amenity", 404, TestName = "5,6,1 - Amenity API call with no parameters missing trailing slash HTTP 404 returned")]
        [TestCase("amenity/", 404, TestName = "5,6,2 - Amenity API call with no parameters including trailing slash HTTP 404 returned")]
        [TestCase("amenity/aaaxxx/axxxxaa/qqqq", 404, TestName = "5,6,4 - Amenity API call with invalid characters parameters - HTTP 404 returned.")]
        [TestCase("amenity/0/0/0", 404, TestName = "5,6,5 - Amenity API call with invalid out of bounds ZERO values parameters - HTTP 404 returned.")]
        [TestCase("amenity/-1/-1/-1", 404, TestName = "5,6,6 - Amenity API call with invalid out of bounds negative values parameters - HTTP 404 returned.")]
        [TestCase("amenity/62.000000/10.083333/250", 404, TestName = "5,6,7 - Amenity API call with outside of Ireland boundary (Norway) parameters HTTP 404 returned.")]
        [TestCase("amenity/48.8534100/2.3488000/1000", 404, TestName = "5,6,8 - Amenity API call with outside of Ireland boundary (Paris, France) parameters HTTP 404 returned.")]
        [TestCase("amenity/768768/979879887/098098098", 404, TestName = "5,6,9 - Amenity API call with massive boundary parameters HTTP 404 returned.")]
        [TestCase("amenity/???/???/???", 404, TestName = "5,6,10 - Amenity API call with question marks as parameters HTTP 404 returned.")]
        public void API_AmenityAPINotFoundCalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode)
        {
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            HttpResponseMessage httpResponse = rClient.Result;
            AssertAll.Execute(
                () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound, "Expected HTTP Code 404 Not Found but instead found " + httpResponse.StatusCode),
                () => Assert.IsTrue(HTTPStatusCode == System.Net.HttpStatusCode.NotFound, "Test Data setup for a " + HTTPStatusCode + " check but executed result produced a " + httpResponse.StatusCode + " HTTP code.  Check test data is correct or API call has changed")
                );
        }
    }

    [TestFixture]
    public class API_BudgetStress
    {

        [Test]
        [Category("API"), Category("BudgetStress"), Category("200HTTPOK")]
        [TestCase("budget/stress", 200, 150000, 10000, 400, 1500, 35, true, 795.200000, 0.0568, TestName = "1,1 - Preparer makes call to Stress Payment budget wizard API")]
        [TestCase("budget/stress", 200, 250000, 20000, 2000, 2000, 32, true, 1343.200000, 0.05840, TestName = "1,2 - Looking makes call to Stress Payment budget wizard API")]
        [TestCase("budget/stress", 200, 350000, 25000, 1600, 1800, 35, true, 1846.000000, 0.0568, TestName = "1,3 - Applying makes call to Stress Payment budget wizard API")]
        [TestCase("budget/stress", 200, 285000, 60000, 2200, 2400, 35, true, 1278.000000, 0.0568, TestName = "1,4 - Mover makes call to Stress Payment budget wizard API")]
        [TestCase("budget/stress", 200, 650000, 120000, 1000, 500, 15, false, 4462.600000, 0.08420, TestName = "1,5 - Upgrader call to savings budget wizard API")]
        [TestCase("budget/stress", 200, 750000, 150000, 3000, 2200, 12, false, 5844.00000, 0.0974, TestName = "1,6 - Max limit 750k makes call to savings budget wizard API")]
        [TestCase("budget/stress", 200, 770000, 195000, 1000, 1750, 22, false, 3915.750000, 0.06810, TestName = "1,7 - Over Max limit makes call 770k Make call to savings budget wizard API")]
        [TestCase("budget/stress", 200, 850000, 600000, 8000, 3000, 5, true, 4830.00000, 0.1932, TestName = "1,8 - Retiree Millionaire Upgrader moving in 18 months makes call to savings budget wizard API")]
        [TestCase("budget/stress", 200, 756000, 300000, 2300, 3300, 7, false, 6653.04000, 0.1459, TestName = "1,9 - 756k Edge case makes call to savings budget wizard API")]

        public void API_BudgetStressAPISuccessCalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode, float Price, float Deposit, float MonthlySavings, float MonthlyRent, int MortgageTerm, bool PassedStressTest, decimal StressAmount, decimal StressRate)
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
        [TestCase("budget/stress", 400, 74999, 8000, 100, 200, 35, "[ArgumentOutOfRangeException] The requested Mortgage amount is outside of the acceptable range", "Parameter name: Mortgage", "", TestName = "2,1 - Mortgage Below Min threshold value, 400 error thrown and error message found")]
        [TestCase("budget/stress", 400, 830500, 80000, 100, 200, 35, "[ArgumentOutOfRangeException] The requested Mortgage amount is outside of the acceptable range", "Parameter name: Mortgage", "", TestName = "2,2 - Mortgage Above Max threshold value")]
        [TestCase("budget/stress", 400, 83000, 8000, 100, 200, 3, "[ArgumentOutOfRangeException] The requested Mortgage Term is outside of the acceptable range", "Parameter name: MortgageTerm", "", TestName = "2,3 - Mortgage Term Below Min value")]
        [TestCase("budget/stress", 400, 83000, 8000, 100, 200, 36, "[ArgumentOutOfRangeException] The requested Mortgage Term is outside of the acceptable range", "Parameter name: MortgageTerm", "", TestName = "2,4 - Mortgage Term Above Max value")]
        [TestCase("budget/stress", 400, 74999, 8000, 100, 200, 35, "[ArgumentOutOfRangeException] The requested Mortgage amount is outside of the acceptable range", "Parameter name: Mortgage", "", TestName = "2,5 - Mortgage Repayment Stress Test Fails as Savings and Amounts Less that Expected Stressed Payment")]
        [TestCase("budget/stress", 400, -750001, 35000, 100, 200, 35, "[ArgumentOutOfRangeException] The price must be greater than zero", "Parameter name: Price", "", TestName = "2,6 - House Price Amount minus value")]
        public void API_BudgetStressAPIBadRequestCalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode, float Price, float Deposit, float MonthlySavings, float MonthlyRent, int MortgageTerm, string ContainsErrorMsg1, string ContainsErrorMsg2, string ContainsErrorMsg3)
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


        [Test]
        [Category("API"), Category("BudgetStress"), Category("404HTTPBadRequest")]
        [TestCase("budget/stress", 404, "{\"Price\": XXXXXX,\"Deposit\": 0,\"MonthlySavings\": 61.2122121,\"MonthlyRent\": 400.0,\"MortgageTerm\": 35}", TestName = "2,7 - House Price Amount is character value")]
        [TestCase("budget/stress", 404, "{\"Price\": £$^&,\"Deposit\": 0,\"MonthlySavings\": 61.2122121,\"MonthlyRent\": 400.0,\"MortgageTerm\": 35}", TestName = "2,8 - House Price Amount is symbol value")]
        [TestCase("budget/stress", 404, "{\"Price\": 88000,\"Deposit\": -300000,\"MonthlySavings\": 61.2122121,\"MonthlyRent\": 400.0,\"MortgageTerm\": 35}", TestName = "2,9 - Deposit Amount minus value")]
        [TestCase("budget/stress", 404, "{\"Price\": 88000,\"Deposit\": XXXXXX,\"MonthlySavings\": 61.2122121,\"MonthlyRent\": 400.0,\"MortgageTerm\": 35}", TestName = "2,10 - Deposit Amount character value")]
        [TestCase("budget/stress", 404, "{\"Price\": 88000,\"Deposit\": !$£^,\"MonthlySavings\": 61.2122121,\"MonthlyRent\": 400.0,\"MortgageTerm\": 35}", TestName = "2,11 - Deposit Amount symbol value")]

        public void API_StressAPINotFoundCallsNonJSONReturned(string strURL, System.Net.HttpStatusCode HTTPStatusCode, string JSONrequest)
        {
            var testData = JSONrequest;

            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.PostAsJsonAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL, testData);
            HttpResponseMessage httpResponse = rClient.Result;
            var sTest = httpResponse.Content.ReadAsStringAsync();
            var testDataResult = sTest.Result;
            AssertAll.Execute(
            () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound, "Found this response " + testDataResult + " returned from API.  Expected HTTP code " + HTTPStatusCode + "  but instead found " + httpResponse.StatusCode)//,
            );
        }



    }

    [TestFixture]
    public class API_BudgetSaving
    {

        [Test]
        [Category("API"), Category("BudgetSavings"), Category("200HTTPOK")]
        [TestCase("budget/saving", 200, 36, true, 150000, 0, 15000, "416.66666666666666666666666667", true, "Error Message expected", TestName = "1,1 - Preparer makes call to savings budget wizard API")]
        [TestCase("budget/saving", 200, 36, true, 250000, 0, 28000, "777.77777777777777777777777778", true, "Error Message expected", TestName = "1,2 - Looking makes call to savings budget wizard API")]
        [TestCase("budget/saving", 200, 36, false, 350000, 0, 70000, "1944.4444444444444444444444444", true, "Error Message expected", TestName = "1,3 - Applying makes call to savings budget wizard API")]
        [TestCase("budget/saving", 200, 36, false, 285000, 60000, 0, "0", true, "Error Message expected", TestName = "1,4 - Mover makes call to savings budget wizard API")]
        [TestCase("budget/saving", 200, 24, false, 650000, 10000, 120000, "5000", true, "Error Message expected", TestName = "1,5 - Upgrader call to savings budget wizard API")]
        [TestCase("budget/saving", 200, 12, false, 750000, 0, 150000, "12500", true, "Error Message expected", TestName = "1,6 - Max limit 750k makes call to savings budget wizard API")]
        [TestCase("budget/saving", 200, 24, false, 770000, 195000, 0, "0", true, "The requested Mortgage amount is outside of the acceptable range", TestName = "1,7 - Over Max limit makes call 770k Make call to savings budget wizard API")]
        [TestCase("budget/saving", 200, 18, false, 850000, 600000, 0, "0", true, "The requested Mortgage amount is outside of the acceptable range", TestName = "1,8 - Retiree Millionaire Upgrader moving in 18 months makes call to savings budget wizard API")]
        [TestCase("budget/saving", 200, 36, false, 756000, 300000, 0, "0", true, "The requested Mortgage amount is outside of the acceptable range", TestName = "1,9 - 756k Edge case makes call to savings budget wizard API")]

        [TestCase("budget/saving", 200, 36, true, 853000, 77000, 71600.000, "1988.8888888888888888888888889", true, "The requested Mortgage amount is outside of the acceptable range", TestName = "2,10 - 756k Pass max threshold (750k) property price to call to savings budget wizard API")]
        public void API_SavingsAPINotFoundCalls(string strURL, System.Net.HttpStatusCode HTTPStatusCode, int Duration, bool BuyerType, decimal TargetPurchasePrice, decimal GiftAmount, decimal TargetDeposit, string TargetSavings, bool IsInvalid, string InvalidMessage)
        {
            var testData = new SavingsBudget();
            testData.Duration = Duration;
            testData.FirstTimeBuyer = BuyerType;
            testData.TargetPurchasePrice = TargetPurchasePrice;
            testData.GiftAmount = GiftAmount;
            testData.IsInvalid = IsInvalid;
            testData.InvalidMessage = InvalidMessage;

            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.PostAsJsonAsync<SavingsBudget>("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL, testData);
            HttpResponseMessage httpResponse = rClient.Result;
            var sTest = httpResponse.Content.ReadAsAsync<SavingsBudget>();
            var testDataResult = sTest.Result;

            AssertAll.Execute(
            () => Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.OK, "Expected HTTP Code " + HTTPStatusCode + "  OK but instead found " + httpResponse.StatusCode),
            () => Assert.IsTrue(testDataResult.IsInvalid == IsInvalid, "Expected IsInvalid flag of " + IsInvalid + " but found " + testDataResult.IsInvalid),
            () => Assert.IsTrue(testDataResult.InvalidMessage == InvalidMessage, "Expected InvalidMessage of " + InvalidMessage + " but found " + testDataResult.InvalidMessage),
            () => Assert.IsTrue(testDataResult.Duration == testData.Duration, "Expected Duration of " + Duration + " but found " + testDataResult.Duration),
            () => Assert.IsTrue(testDataResult.FirstTimeBuyer == testData.FirstTimeBuyer, "Expected First time Buyer to be " + BuyerType + " but found " + testDataResult.FirstTimeBuyer),
            () => Assert.IsTrue(testDataResult.GiftAmount == testData.GiftAmount, "Expected Gift Amount of " + GiftAmount + " but found" + testDataResult.GiftAmount),
            () => Assert.IsTrue(testDataResult.TargetDeposit == TargetDeposit, "Expected TargetDeposit Amount of " + TargetDeposit + " but found " + testDataResult.TargetDeposit),
            () => Assert.IsTrue(testDataResult.TargetPurchasePrice == testData.TargetPurchasePrice, "Expected TargetPurchasePrice of " + TargetPurchasePrice + "but found " + testDataResult.TargetPurchasePrice),
            () => Assert.IsTrue(testDataResult.TargetSavings == Convert.ToDecimal(TargetSavings), "Expected TargetSavings Amount of " + Convert.ToDecimal(TargetSavings) + " but found " + testDataResult.TargetSavings)
            );
        }


        [TestCase("budget/saving", 400, "{\"Duration\":36,\"FirstTimeBuyer\":false,\"TargetDeposit\":15000,\"TargetPurchasePrice\":-756000,\"TargetSavings\":300000}", TestName = "2,1 - Pass minus property price call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":12,\"FirstTimeBuyer\":1,\"TargetDeposit\":15000}", TestName = "2,2 - Pass 1 First Buyer (not bool) call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":12,\"FirstTimeBuyer\":-1,\"TargetDeposit\":15000}", TestName = "2,3 - Pass minus 1 First Buyer (not bool) call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":12,\"FirstTimeBuyer\":false,\"TargetDeposit\":15000,\"GiftAmount\":-15000}", TestName = "2,4 - Pass minus Gift Amount to call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":-36 ,\"FirstTimeBuyer\":true,\"TargetDeposit\":150001,\"TargetPurchasePrice\":75000,\"TargetSavings\":1500}", TestName = "2,5 - Pass minus Duration to call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":36 ,\"FirstTimeBuyer\":true,\"TargetDeposit\":15000,\"TargetPurchasePrice\":0,\"TargetSavings\":1500}", TestName = "2,6 - Pass 0 property price to call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":12,\"FirstTimeBuyer\":-2,\"TargetDeposit\":15000}", TestName = "2,7 - Pass minus 2 First Buyer value to call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":0,\"FirstTimeBuyer\":true,\"TargetDeposit\":15000,\"TargetPurchasePrice\":75000,\"TargetSavings\":1500}", TestName = "2,8 - Pass 0 Duration to call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":12,\"FirstTimeBuyer\":true,\"TargetDeposit\":15000,\"TargetPurchasePrice\":75000,\"TargetSavings\":1500}", TestName = "2,9 - Pass min threshold (75k) property price to call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":37,\"FirstTimeBuyer\":true,\"TargetDeposit\":15000,\"TargetPurchasePrice\":85000,\"TargetSavings\":1500}", TestName = "2,11 - Pass 37 Duration to call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":99999,\"FirstTimeBuyer\":true,\"TargetDeposit\":15000,\"TargetPurchasePrice\":85000,\"TargetSavings\":1500}", TestName = "2,12 - Pass 99999 Duration to call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":AAAA,\"FirstTimeBuyer\":true,\"TargetDeposit\":1500,\"TargetPurchasePrice\":85000,\"TargetSavings\":1500}", TestName = "2,13 - Pass AAAA Duration to call to savings budget wizard API")]
        [TestCase("budget/saving", 400, "{\"Duration\":£$^& ,\"FirstTimeBuyer\":true,\"TargetDeposit\":150001,\"TargetPurchasePrice\":75000,\"TargetSavings\":1500}", TestName = "2,14 -  Pass £$^& Duration to call to savings budget wizard API")]

        public void API_SavingsAPINotFoundCallsNonJSONReturned(string strURL, System.Net.HttpStatusCode HTTPStatusCode, string JSONrequest)
        {
            // assign the built up JSON that contains the bad request parameters
            var testData = JSONrequest;
            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.PostAsJsonAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL, testData);
            HttpResponseMessage httpResponse = rClient.Result;
            //read Response as string as the response won't be JSON it will be a HTTP paged error    
            var sTest = httpResponse.Content.ReadAsStringAsync();
            var testDataResult = sTest.Result;

            AssertAll.Execute(
            () => Assert.IsTrue(httpResponse.StatusCode == HTTPStatusCode, "Found this response " + testDataResult + " returned from API.  Expected HTTP code " + HTTPStatusCode + "  but instead found " + httpResponse.StatusCode)//,
            //() => Assert.IsTrue(testDataResult.Duration == testData.Duration, "Expected Duration of " + Duration + " but found " + testDataResult.Duration),
            //() => Assert.IsTrue(testDataResult.FirstTimeBuyer == testData.FirstTimeBuyer, "Expected First time Buyer to be " + BuyerType + " but found " + testDataResult.FirstTimeBuyer),
            //() => Assert.IsTrue(testDataResult.GiftAmount == testData.GiftAmount, "Expected Gift Amount of " + GiftAmount + " but found" + testDataResult.GiftAmount),
            //() => Assert.IsTrue(testDataResult.TargetDeposit == TargetDeposit, "Expected TargetDeposit Amount of " + TargetDeposit + " but found " + testDataResult.TargetDeposit),
            //() => Assert.IsTrue(testDataResult.TargetPurchasePrice == testData.TargetPurchasePrice, "Expected TargetPurchasePrice of " + TargetPurchasePrice + "but found " + testDataResult.TargetPurchasePrice),
            //() => Assert.IsTrue(testDataResult.TargetSavings == Convert.ToDecimal(TargetSavings), "Expected TargetSavings Amount of " + TargetSavings + " but found " + testDataResult.TargetSavings)
            );
        }
    }

    [TestFixture]
    public class API_CheckList
    {

        [Test]
        [TestCase("system/checklist/TestChecklist/Template", 200, "TestChecklist", "CheckListTemplate", "Test Checklist", "Sub Text", "Disclaimer Text", TestName = "1,1 -  Call the test template")]
        public void API_CheckListAPIReturned(string strURL, System.Net.HttpStatusCode HTTPStatusCode, string Id, string Type, string DisplayHeader, string IntroductionText, string FooterText)
        {
            var testData = new CheckList();
            testData.Id = Id;
            testData.Type = Type;
            testData.DisplayHeader = DisplayHeader;
            testData.IntroductionText = IntroductionText;
            testData.FooterText = FooterText;


            RestClient API_client = new RestClient();
            var client = API_client.client("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL);
            // is the server available??            
            var rClient = client.GetAsync("http://homeappapihost-dev.elasticbeanstalk.com/" + strURL); //, testData);
            HttpResponseMessage httpResponse = rClient.Result;
            //read Response as string as the response won't be JSON it will be a HTTP paged error    
            var sTest = httpResponse.Content.ReadAsAsync<CheckList>();
            var testDataResult = sTest.Result;

            AssertAll.Execute(
            () => Assert.IsTrue(httpResponse.StatusCode == HTTPStatusCode, "Found this response " + testDataResult + " returned from API.  Expected HTTP code " + HTTPStatusCode + "  but instead found " + httpResponse.StatusCode),
            () => Assert.IsTrue(testDataResult.Id == testData.Id, "Expected Id of " + Id + " but found " + testDataResult.Id),
            () => Assert.IsTrue(testDataResult.Type == testData.Type, "Expected Type to be " + Type + " but found " + testDataResult.Type),
            () => Assert.IsTrue(testDataResult.DisplayHeader == testData.DisplayHeader, "Expected DisplayHeader of " + DisplayHeader + " but found" + testDataResult.DisplayHeader),
            () => Assert.IsTrue(testDataResult.IntroductionText == IntroductionText, "Expected IntroductionText of " + IntroductionText + " but found " + testDataResult.IntroductionText),
            () => Assert.IsTrue(testDataResult.FooterText == testData.FooterText, "Expected FooterText of " + FooterText + "but found " + testDataResult.FooterText) //,
            //() => Assert.IsTrue(testDataResult.TargetSavings == Convert.ToDecimal(TargetSavings), "Expected TargetSavings Amount of " + TargetSavings + " but found " + testDataResult.TargetSavings)
            );

        }
    }
}
