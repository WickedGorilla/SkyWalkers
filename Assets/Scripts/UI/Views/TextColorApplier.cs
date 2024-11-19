namespace UI.Views
{
    public static class TextColorApplier
    {
        private const string PurpleCodeColor = "FF39FF";
        
        public static string ApplyPurpleColor(this string text) 
            => GetTextWithColor(text, PurpleCodeColor);

        private static string GetTextWithColor(string text, string color) 
            => $"<color=#{color}>{text}</color>";
    }
}