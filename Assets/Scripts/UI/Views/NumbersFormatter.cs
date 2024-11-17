using System.Globalization;

namespace UI.Views
{
    public static class NumbersFormatter
    {
        public  static string GetCoinsCountVariant(int coinsCount) 
            => $"{SpritesAtlasCode.Coin} { coinsCount.ToString("N0", CultureInfo.InvariantCulture)}";
    }
}