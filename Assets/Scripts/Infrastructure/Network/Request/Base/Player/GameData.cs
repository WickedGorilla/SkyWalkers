using System;
using Infrastructure.Network.Response.Player;

namespace Infrastructure.Network.Request.Base.Player
{
    [Serializable]
    public class GameData
    {
        public string Token;
        public BalanceUpdate BalanceUpdate;
        public PerksResponse Perks;
        public int AutoTapCoins;
        public ReferralInfo ReferralInfo;
        public bool ClaimBonus;
        public int TappedCoinsBeforeMiniGame;
        
        public GameData(int coins, int energy, int playPass, int boosts)
        {
            BalanceUpdate = new BalanceUpdate
            {
                Coins = coins,
                Energy = energy,
                PlayPass = playPass,
                Boosts = boosts
            };

            Perks = new PerksResponse();
        }
    }
    
    [Serializable]
    public struct ReferralInfo
    {
        public string ReferralLink;
        public int NewAddedValue;
        public int TotalCoins;
        public int CountReferrals;
    }
}