using System;

namespace Cqrs.Commons
{
    public interface IValidatorService
    {
        void Validate<T>(T item);
        void Validate(object item, Type t);
    }
}
