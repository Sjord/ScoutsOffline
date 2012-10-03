namespace ScoutsOffline.Sol
{
    public class AuthenticateResponse : SolResponse
    {
        public AuthenticateResponse(Http.Response response) : base(response)
        {
            LoggedIn = response.Content.Contains("/rs/user/?perform=logout");
        }

        public bool LoggedIn { get; set; }
    }
}
