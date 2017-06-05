using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cqrs.Commons.Infrastracture
{
    public class ValidatorService : IValidatorService
    {
        private readonly Dictionary<Type, List<IValidator>> _validators;
        private readonly Dictionary<Type, List<MethodInfo>> _validatorMethods;

        public ValidatorService(IEnumerable<IValidator> validators)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            _validators = new Dictionary<Type, List<IValidator>>();
            _validatorMethods = new Dictionary<Type, List<MethodInfo>>();
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
                        _validatorMethods[paramaterType] = new List<MethodInfo>();
                    }
                    _validators[paramaterType].Add(item);
                    _validatorMethods[paramaterType].Add(method);
                }
            }
        }

        public void Validate<T>(T item)
        {
            var type = typeof(T);
            Validate(item, type);
        }

        public void Validate(object item, Type t)
        {
            if (!_validators.ContainsKey(t)) return;
            for (var i = 0; i < _validators[t].Count; i++)
            {
                var instance = _validators[t][i];
                var method = _validatorMethods[t][i];
                method.Invoke(instance, new[] { item });
            }
        }
    }
}
