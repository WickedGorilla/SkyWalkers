using System;
using UnityEngine.Serialization;

namespace Infrastructure.Network.Response.Player
{
    [Serializable]
    public class GameData
    {
        public string Token;
        
        public BalanceUpdate BalanceUpdate;
        public PerksInfo PerksInfo;

        public GameData(int coins, int energy, int playPass, int boosts)
        {
            BalanceUpdate = new BalanceUpdate
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