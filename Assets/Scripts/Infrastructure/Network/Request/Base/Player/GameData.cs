using System;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public class GameData
    {
        public string Token;
     
        public BalanceData BalanceData;
        public PerksInfo PerksInfo;

        public GameData(int coins, int energy, int playPass, int boosts)
        {
            BalanceData = new BalanceData
            {
                Coins = coins,
                Energy = energy,
                PlayPass = playPass,
                Boosts = boosts
            };
            
            PerksInfo = new PerksInfo();
        }
    }
}