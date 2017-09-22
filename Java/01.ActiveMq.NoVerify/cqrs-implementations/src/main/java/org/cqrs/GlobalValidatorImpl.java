package org.cqrs;

import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;
import java.util.function.Consumer;
import javax.inject.Inject;
import javax.inject.Named;
import org.utils.AggregateException;

@Named("globalValidator")
public class GlobalValidatorImpl extends GlobalValidator {

    @Inject
    public GlobalValidatorImpl(List<Validator> validators) {
        for (int i = 0; i < validators.size(); i++) {
            Validator validator = validators.get(i);
            validator.Register(this);

        }
    }

    private final Hashtable<Class, ArrayList<Consumer<Object>>> _validatorFunctions = new Hashtable<>();

    @Override
    public void RegisterValidator(Consumer<Object> validateFunction, Class typeToValidate) {
        if (!_validatorFunctions.containsKey(typeToValidate)) {
            _validatorFunctions.put(typeToValidate, new ArrayList<>());
        }
        _validatorFunctions.get(typeToValidate).add(validateFunction);
    }

    @Override
    public boolean Validate(Object toValidate, boolean throwOnError) throws AggregateException {
        if (toValidate == null) {
            return false;
        }
        ArrayList<Exception> listOfExceptions = new ArrayList<>();
        Class typeToValidate = toValidate.getClass();
        if (_validatorFunctions.containsKey(typeToValidate)) {
            ArrayList<Consumer<Object>> validateFunction = _validatorFunctions.get(typeToValidate);
            for (int i = 0; i < validateFunction.size(); i++) {
                try {
                    validateFunction.get(i).accept(toValidate);
                } catch (Exception ex) {
                    listOfExceptions.add(ex);
                }
            }
        }
        if (throwOnError && listOfExceptions.size() > 0) {
            throw new AggregateException(listOfExceptions);
        }
        return listOfExceptions.isEmpty();
    }
}
