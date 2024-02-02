namespace StresslessUI.DataModels
{
    internal class OAuthTokenModel
    {
        public string expires { get; set; }
        public string tokenType { get; set; }
        public string token { get; set; }

        public static OAuthTokenModel _model = new();
    }
}
