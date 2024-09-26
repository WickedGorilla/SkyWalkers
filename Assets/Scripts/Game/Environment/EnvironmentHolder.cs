namespace Game.Environment
{
    public class EnvironmentHolder : IEnvironmentHolder
    {
        public EnvironmentObjects Environment { get; set; }

        public void Hold(EnvironmentObjects environment) 
            => Environment = environment;
    }
}