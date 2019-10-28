using NServiceBus;

namespace Cruise.Commands
{
    public class CreateCruise:ICommand
    {
        public CreateCruise(string name)
        {
            Name = name;
        }
        public string Name{ get; set; }
    }
}
