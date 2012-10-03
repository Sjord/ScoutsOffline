namespace ScoutsOffline.Sol
{
    using System.Linq;
    using ScoutsOffline.Http;

    class Authenticator
    {
        private Browser browser;
        private string baseUrl;

        public Authenticator(Browser browser, string baseUrl)
        {
            this.browser = browser;
            this.baseUrl = baseUrl;
        }

        public Response Authenticate(string username, string password)
        {
            var url = string.Format("{0}rs/user/", this.baseUrl);
            var loginPageRequest = new Request(url);
            var loginPageResponse = browser.DoRequest(loginPageRequest);

            var formValues = loginPageResponse.GetForms().Single().Values;
            formValues.Update("userid", username);

            var loginPageSubmit = new PostRequest(url, formValues);
            var openIdPage = browser.DoRequest(loginPageSubmit);

            formValues = openIdPage.GetForms().Single().Values;
            formValues.Update("openid_password", password);
            formValues.Update("openid_action", "Login");

            var openIdSubmit = new PostRequest(openIdPage.ResponseUri, formValues);
            var response = browser.DoRequest(openIdSubmit);

            return response;
        }
    }
}
