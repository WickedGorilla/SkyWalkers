using Infrastructure.Network.Request.Base;

namespace Infrastructure.Network.Request.UpdateGameData
{
    public class UpdateGameDataRequest : ServerRequest
    {
        public static readonly UpdateGameDataRequest Empty = new();
    }
}