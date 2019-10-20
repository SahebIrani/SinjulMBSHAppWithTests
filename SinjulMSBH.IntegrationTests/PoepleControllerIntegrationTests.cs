using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Net.Http.Headers;

using SinjulMSBH.WebUI;

using Xunit;

namespace SinjulMSBH.IntegrationTests
{
    public class PoepleControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        public PoepleControllerIntegrationTests(TestingWebAppFactory<Startup> factory) => HttpClient = factory.CreateClient();
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
            //    { "Name", "New Person" },
            //    { "Age", "25" }
            //};

            (string fieldValue, string cookieValue) = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, "/People/Create");

            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, cookieValue).ToString());

            Dictionary<string, string> formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, fieldValue },{ "Name", "Sinjul MSBH" },{ "Age", "28" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            HttpResponseMessage response = await HttpClient.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Account number is required", responseString);
        }

        [Fact]
        public async Task Create_WhenPOSTExecuted_ReturnsToIndexView()
        {
            HttpResponseMessage initResponse = await HttpClient.GetAsync("/People/Create");

            //var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            //var postRequest = new HttpRequestMessage(HttpMethod.Post, "/People/Create");

            //postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());

            //var modelData = new Dictionary<string, string>
            //{
            //    { AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue },
            //    { "Name", "New Person" },
            //    { "Age", "25" },
            //    { "AccountNumber", "214-5874986532-21" }
            //};

            //(string fieldValue, string cookieValue) antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);
            (string fieldValue, string cookieValue) = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, "/People/Create");

            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, cookieValue).ToString());

            Dictionary<string, string> modelData = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, fieldValue },
                { "Name", "Sinjul MSBH" },
                { "Age", "27" },
                { "AccountNumber", "313-5874986532-22" }
            };

            postRequest.Content = new FormUrlEncodedContent(modelData);

            HttpResponseMessage response = await HttpClient.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Sinjul MSBH", responseString);
            Assert.Contains("313-5874986532-22", responseString);
        }
    }
}
