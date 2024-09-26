namespace Game.Player
{
    public class PlayerHolder : IPlayerHolder
    {
        public PlayerAnimation Player { get; private set; }

        public void Hold(PlayerAnimation player) 
            => Player = player;
    }
}