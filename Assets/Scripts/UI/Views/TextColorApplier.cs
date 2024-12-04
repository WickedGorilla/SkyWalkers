namespace UI.Views
{
    public static class TextColorApplier
    {
        private const string PurpleCodeColor = "FF39FF";
        private const string WhiteCodeColor = "FFFFFF";
        
        public static string ApplyPurpleColor(this string text) 
            => GetTextWithColor(text, PurpleCodeColor);

        public static string ApplyWhiteColor(this string text) 
            => GetTextWithColor(text, WhiteCodeColor);
        
        private static string GetTextWithColor(string text, string color) 
            => $"<color=#{color}>{text}</color>";
    }
}