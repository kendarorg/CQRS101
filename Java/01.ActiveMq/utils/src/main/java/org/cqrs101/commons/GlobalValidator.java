package org.cqrs101.commons;

import java.util.function.Consumer;
import org.cqrs101.utils.AggregateException;

public abstract class GlobalValidator {

    public abstract void RegisterValidator(Consumer<Object> validate, Class validateType);

    public abstract boolean Validate(Object toValidate, boolean throwOnError) throws AggregateException;

    public boolean Validate(Object toValidate) throws AggregateException {
        return Validate(toValidate, false);
    }
}
