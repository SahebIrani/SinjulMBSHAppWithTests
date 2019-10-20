using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Net.Http.Headers;

using SinjulMSBH.WebUI;

using Xunit;

namespace SinjulMSBH.IntegrationTests
{
    public class PeopleControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        public PeopleControllerIntegrationTests(TestingWebAppFactory<Startup> factory) => HttpClient = factory.CreateClient();
        private readonly HttpClient HttpClient;

        [Fact]
        public async Task Index_WhenCalled_ReturnsApplicationForm()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/People");

            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Sinjul", responseString);
            Assert.Contains("MSBH", responseString);
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsCreateForm()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/People/Create");

            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Please provide a new person data .. !!!!", responseString);
        }

        [Fact]
        public async Task Create_SentWrongModel_ReturnsViewWithErrorMessages()
        {
            var initResponse = await HttpClient.GetAsync("/People/Create");

            //var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            //var postRequest = new HttpRequestMessage(HttpMethod.Post, "/People/Create");

            //postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());

            //var formModel = new Dictionary<string, string>
            //{
            //    { AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue },
            //    { "Name", "New Employee" },
            //    { "Age", "25" }
            //};

            var (fieldValue, cookieValue) = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/People/Create");

            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, cookieValue).ToString());

            var formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, fieldValue },{ "Name", "New Employee" },{ "Age", "25" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await HttpClient.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Account number is required", responseString);
        }

        [Fact]
        public async Task Create_WhenPOSTExecuted_ReturnsToIndexView()
        {
            var initResponse = await HttpClient.GetAsync("/People/Create");
            var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/People/Create");

            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());

            var modelData = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue },
                { "Name", "New Employee" },
                { "Age", "25" },
                { "AccountNumber", "214-5874986532-21" }
            };

            postRequest.Content = new FormUrlEncodedContent(modelData);

            var response = await HttpClient.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("New Employee", responseString);
            Assert.Contains("214-5874986532-21", responseString);
        }
    }
}
