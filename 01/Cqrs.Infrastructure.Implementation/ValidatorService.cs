using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cqrs.Commons.Infrastracture
{
    public class ValidatorService : IValidatorService
    {
        private readonly Dictionary<Type, List<IValidator>> _validators;
        
        public ValidatorService(IEnumerable<IValidator> validators)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            _validators = new Dictionary<Type, List<IValidator>>();
            foreach (var item in validators)
            {
                var methods = item.GetType().GetMethods(bindingFlags).Where(m => m.Name == "Validate");
                foreach (var method in methods)
                {
                    var parameter = method.GetParameters().First();
                    var paramaterType = parameter.ParameterType;
                    if (!_validators.ContainsKey(paramaterType))
                    {
                        _validators[paramaterType] = new List<IValidator>();
                    }
                    _validators[paramaterType].Add(item);
                }
            }
        }

        public void Validate<T>(T item) where T : class, ICommand
        {
            IEnumerable<IValidator<T>> validators = GetValidatorsForCommand<T>();
            foreach (var validator in validators)
            {
                validator.Validate(item);
            }
        }

        private IEnumerable<IValidator<T>> GetValidatorsForCommand<T>() where T : class, ICommand
        {
            var t = typeof(T);
            if (!_validators.ContainsKey(t)) yield break;
            for (var i = 0; i < _validators[t].Count; i++)
            {
                yield return (IValidator<T>)_validators[t][i];
            }
        }
    }
}
