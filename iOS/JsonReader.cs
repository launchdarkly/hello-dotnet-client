namespace LaunchDarkly.Xamarin.iOS
{
    public static class JSONReader
    {
        public static string FeatureFlagJSON()
        {
            string text = System.IO.File.ReadAllText("FeatureFlag.json");
            return text;
        }
    }
}
