namespace UI.Views
{
    public static class SpritesAtlasCode
    {
        public const string Coin = "<sprite name=\"coin\">";
        public const string Star = "<sprite name=\"star\">";
        public const string Magic = "<sprite name=\"magic\">";
        public const string Lock = "<sprite name=\"lock\">";
        
        public static string GetCurrentCurrencyCode(bool isDonat) 
            => isDonat ? Star : Coin;
    }
}