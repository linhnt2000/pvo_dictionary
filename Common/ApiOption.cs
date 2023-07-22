namespace thu6_pvo_dictionary.Common
{
    public class ApiOption
    {
        public string StringConnection { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public string BaseUrl { get; set; }
        public string AuthSecret { get; set; }

        // Secret key for activation token
        public string ActivationSecret { get; set; }
    }
}
