namespace Cqrs.Commons
{
    public interface IValidator
    {
    }

    public interface IValidator<in T> : IValidator where T:class
    {
        bool Validate(T item);
    }
}
