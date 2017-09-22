package org.cqrs101.utils;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class AggregateException extends Exception {

    private final List<Exception> secondaryExceptions;

    public AggregateException(String message, Exception... others) {
        super(message);
        this.secondaryExceptions = new ArrayList<>();
        this.secondaryExceptions.addAll(Arrays.asList(others));
    }

    public AggregateException(Exception... others) {
        super();
        this.secondaryExceptions = new ArrayList<>();
        this.secondaryExceptions.addAll(Arrays.asList(others));
    }

    public AggregateException(String message, List<Exception> exceptions) {
        super(message);
        this.secondaryExceptions = new ArrayList<>();
        for (int i = 0; i < exceptions.size(); i++) {
            this.secondaryExceptions.add(exceptions.get(i));
        }
    }

    public AggregateException(List<Exception> exceptions) {
        super();
        this.secondaryExceptions = new ArrayList<>();
        for (int i = 0; i < exceptions.size(); i++) {
            this.secondaryExceptions.add(exceptions.get(i));
        }
    }

    public Throwable[] getAllExceptions() {

        int start = 0;
        int size = secondaryExceptions.size();
        final Throwable primary = getCause();
        if (primary != null) {
            start = 1;
            size++;
        }

        Throwable[] all = new Exception[size];

        if (primary != null) {
            all[0] = primary;
        }

        Arrays.fill(all, start, all.length, secondaryExceptions);
        return all;
    }

}
