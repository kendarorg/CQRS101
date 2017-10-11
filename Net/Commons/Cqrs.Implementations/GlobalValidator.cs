using System;
using System.Linq;
using System.Collections.Generic;

namespace Cqrs
{
    public class GlobalValidator : IGlobalValidator
    {
        public GlobalValidator(IList<IValidator> validators)
        {
            for(int i = 0; i < validators.Count(); i++)
            {
                var validator = validators[i];
                validator.Register(this);

            }
        }

        private Dictionary<Type, List<Action<object>>> _validatorFunctions = new Dictionary<Type, List<Action<object>>>();
        
        public void RegisterValidator(Action<object> validateFunction, Type typeToValidate)
        {
            if (!_validatorFunctions.ContainsKey(typeToValidate))
            {
                _validatorFunctions.Add(typeToValidate, new List<Action<object>>());
            }
            _validatorFunctions[typeToValidate].Add(validateFunction);
        }

        public bool Validate(object toValidate, bool throwOnError = false)
        {
            if (toValidate == null) return false;
            var listOfExceptions = new List<Exception>();
            var typeToValidate = toValidate.GetType();
            if (_validatorFunctions.ContainsKey(typeToValidate))
            {
                var validateFunction = _validatorFunctions[typeToValidate];
                for (var i = 0; i < validateFunction.Count; i++)
                {
                    try
                    {
                        validateFunction[i](toValidate);
                    }
                    catch (Exception ex)
                    {
                        listOfExceptions.Add(ex);
                    }
                }
            }
            if (throwOnError && listOfExceptions.Count > 0)
            {
                throw new AggregateException(listOfExceptions.ToArray());
            }
            return listOfExceptions.Count == 0;
        }
    }
}
