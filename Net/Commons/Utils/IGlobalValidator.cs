using System;
using Utils;

namespace Cqrs
{
    public interface IGlobalValidator:ISingleton
    {
        void RegisterValidator(Action<object> validate,Type typeToValidate);
        bool Validate(object toValidate, bool throwOnError = false);
    }
}
