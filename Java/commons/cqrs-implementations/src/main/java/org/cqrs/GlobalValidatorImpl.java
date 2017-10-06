package org.cqrs;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Consumer;
import javax.inject.Inject;
import javax.inject.Named;
import org.cqrs101.commons.GlobalValidator;
import org.cqrs101.commons.Validator;
import org.cqrs101.utils.AggregateException;

@Named("globalValidator")
public class GlobalValidatorImpl extends GlobalValidator {

    @Inject
    public GlobalValidatorImpl(List<Validator> validators) {
        for (int i = 0; i < validators.size(); i++) {
            Validator validator = validators.get(i);
            validator.Register(this);

        }
    }

    private final ConcurrentHashMap<Class, ArrayList<Consumer<Object>>> validatorFunctions = new ConcurrentHashMap<>();

    @Override
    public void registerValidator(Consumer<Object> validateFunction, Class typeToValidate) {
        validatorFunctions.putIfAbsent(typeToValidate, new ArrayList<>());
        validatorFunctions.get(typeToValidate).add(validateFunction);
    }

    @Override
    public boolean validate(Object toValidate, boolean throwOnError) throws AggregateException {
        if (toValidate == null) {
            return false;
        }
        ArrayList<Exception> listOfExceptions = new ArrayList<>();
        Class typeToValidate = toValidate.getClass();
        if (validatorFunctions.containsKey(typeToValidate)) {
            ArrayList<Consumer<Object>> validateFunction = validatorFunctions.get(typeToValidate);
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
