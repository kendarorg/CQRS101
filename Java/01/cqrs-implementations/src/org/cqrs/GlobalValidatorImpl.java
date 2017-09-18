package org.cqrs;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Hashtable;
import java.util.List;
import java.util.function.Consumer;
import org.commons.AggregateException;
import org.cqrs.GlobalValidator;
import org.cqrs.Validator;

public class GlobalValidatorImpl extends GlobalValidator {

    public GlobalValidatorImpl(List<Validator> validators) {
        for (int i = 0; i < validators.size(); i++) {
            Validator validator = validators.get(i);
            validator.Register(this);

        }
    }

    private Hashtable<Class, ArrayList<Consumer<Object>>> _validatorFunctions = new Hashtable<>();

    public void RegisterValidator(Consumer<Object> validateFunction, Class typeToValidate) {
        if (!_validatorFunctions.containsKey(typeToValidate)) {
            _validatorFunctions.put(typeToValidate, new ArrayList<Consumer<Object>>());
        }
        _validatorFunctions.get(typeToValidate).add(validateFunction);
    }

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
        return listOfExceptions.size() == 0;
    }
}
