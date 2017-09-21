package org.tasks;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.HashMap;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.ImportResource;

@SpringBootApplication
@ImportResource("classpath:app-config.xml")
public class Program {

    public static void main(String[] args) {
        try {
            String port = "9000";
            String baseAddress = "http://localhost:" + port + "/";

            SpringApplication application = new SpringApplication(Program.class);
            Map<String, Object> map = new HashMap<>();
            map.put("SERVER_PORT", port);
            application.setDefaultProperties(map);
            application.run(args);
        } catch (Exception ex) {
            Logger.getLogger(Program.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
}