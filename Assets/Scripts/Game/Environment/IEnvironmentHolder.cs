namespace Game.Environment
{
    public interface IEnvironmentHolder
    {
        EnvironmentObjects Environment { get; set; }
 
        void Hold(EnvironmentObjects environment);
    }
}