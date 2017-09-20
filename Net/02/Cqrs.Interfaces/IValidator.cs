using Utils;

namespace Cqrs
{
    public interface IValidator : ISingleton
    {
        void Register(IGlobalValidator globalValidator);
    }
}
