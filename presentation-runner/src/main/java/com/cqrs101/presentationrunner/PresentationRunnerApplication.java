package com.cqrs101.presentationrunner;

import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.*;
import java.util.stream.Stream;

@SpringBootApplication
public class PresentationRunnerApplication implements CommandLineRunner {

	public static void main(String[] args) {
		SpringApplication.run(PresentationRunnerApplication.class, args);
	}

	@Override
	public void run(String... args) throws Exception {
	    String fileName = args[0];
        try (Stream<String> stream = Files.lines(Paths.get(fileName))) {
            stream.forEach((k)->execute(k));
        }
        //#comment
        //fileContent=read,file
        //result=call,http://address,fileContent
        //show,result
        //pause
        //clear
        //show
		//result=ask,
        //fileContent=read,file
        //fileContent=replace,fileContent,what,with

	}

	private Map<String,String> variables = new HashMap<>();

    private String execute(String line) {
	    line = line.trim();
	    if(isAssignement(line)){
	        int equals = line.indexOf('=');
            String variableValue = line.substring(0,equals);
            String variableName = line.substring(equals);

        }else{

        }
        return "";
    }

    private boolean isAssignement(String line) {
        return line.indexOf('=')>=0;
    }
}
