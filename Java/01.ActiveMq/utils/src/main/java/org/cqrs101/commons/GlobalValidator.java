package org.cqrs101.commons;

import java.util.function.Consumer;
import org.cqrs101.utils.AggregateException;

public abstract class GlobalValidator {

    public abstract void registerValidator(Consumer<Object> validate, Class validateType);

    public abstract boolean validate(Object toValidate, boolean throwOnError) throws AggregateException;

    public boolean validate(Object toValidate) throws AggregateException {
        return validate(toValidate, false);
    }
}
