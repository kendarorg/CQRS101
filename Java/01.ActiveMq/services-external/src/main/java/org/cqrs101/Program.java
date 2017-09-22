package org.cqrs101;

import java.util.logging.Level;
import java.util.logging.Logger;
import org.cqrs.Bus;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.ApplicationContext;
import org.springframework.context.annotation.ImportResource;

@SpringBootApplication
@ImportResource("classpath:app-config.xml")
public class Program {


    public static void main(String[] args) {
        try {
            SpringApplication application = new SpringApplication(Program.class);
            ApplicationContext ctx = application.run(args);
            
            Bus bus = (Bus)ctx.getBean("bus");
        } catch (Exception ex) {
            Logger.getLogger(Program.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
}
