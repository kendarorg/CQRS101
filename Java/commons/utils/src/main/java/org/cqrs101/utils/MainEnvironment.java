package org.cqrs101.utils;


import javax.inject.Inject;
import javax.inject.Named;
import org.springframework.core.env.Environment;

import java.util.Locale;
import java.util.concurrent.ConcurrentHashMap;

@Named("mainEnvironment")
public class MainEnvironment {
    private final Environment environment;
    private final ConcurrentHashMap<String,String> data = new ConcurrentHashMap<>();

    @Inject
    public MainEnvironment(Environment environment){
        this.environment = environment;
    }


    public String getProperty(String s) {
        String st = s.toUpperCase(Locale.ROOT);
        if(data.containsKey(st)){
            return data.get(st);
        }
        if(this.environment==null)return null;
        return environment.getProperty(s);
    }
    public void setProperty(String s,String value) {
        data.put(s.toUpperCase(Locale.ROOT),value);
    }
}
