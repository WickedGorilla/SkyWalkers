using Infrastructure.Network.Response.Player;

namespace Infrastructure.Network.Request.UpdateGameData
{
    public struct UpdateGameDataResponse
    {
        public BalanceUpdate BalanceUpdate;
        public PerksResponse PerksUser;
        public int TappedCoinsBeforeMiniGame;
    }
}