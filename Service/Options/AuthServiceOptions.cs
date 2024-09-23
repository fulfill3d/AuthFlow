namespace AuthFlow.Service.Options
{
    public class AuthServiceOptions
    {
        public string PostRegisterRedirectUri { get; set; }
        public string PostUpdateRedirectUri { get; set; }
        public string UnauthorizedRedirectUri { get; set; }
    }
}