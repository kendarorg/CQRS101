using Commons;

namespace Cqrs.Commons
{
    public interface IValidatorService : IService
    {
        void Validate<T>(T item) where T : class, ICommand;
    }
}
