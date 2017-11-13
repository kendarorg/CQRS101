package utils;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import javax.ws.rs.client.*;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import java.io.IOException;
import java.util.List;
import java.util.Locale;

public class RestUtil {


    private static final ObjectMapper mapper = new ObjectMapper();


    public static <T> T askItem(int port,String path,String verb,Object param) {

        try {
            Client client = ClientBuilder.newClient();
            WebTarget target = client.target("http://localhost:"+port).path(path);
            Invocation.Builder invocationBuilder = target.request(MediaType.APPLICATION_JSON);

            String paramString = "";
            if(param!=null){
                paramString = mapper.writeValueAsString(param);
            }
            Response response = null;
            if(verb==null) verb = "get";
            verb = verb.toLowerCase(Locale.ROOT);
            if(verb == "post") {
                response = invocationBuilder.post(Entity.json(paramString));
            }else if(verb == "delete") {
                response = invocationBuilder.delete();
            }else if(verb == "put") {
                response = invocationBuilder.put(Entity.json(paramString));
            }else{
                response = invocationBuilder.get();
            }
            String responseStr = response.readEntity(String.class);
            return mapper.readValue(responseStr, new TypeReference<T>(){});
        } catch (IOException ex) {
            return null;
        }

    }
}
