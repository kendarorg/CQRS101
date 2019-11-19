namespace Cruise.Commands
{
    public class CreateCruise
    {
        public CreateCruise(string name)
        {
            Name = name;
        }
        public string Name{ get; }
    }
}
