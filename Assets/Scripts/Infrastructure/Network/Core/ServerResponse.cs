namespace Infrastructure.Network
{
    public class ServerResponse<TResponse>
    {
        public bool Success;
        public TResponse Data;
    }
}