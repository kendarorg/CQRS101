package utils;

import com.fasterxml.jackson.databind.ObjectMapper;

import javax.ws.rs.client.*;
import javax.ws.rs.core.HttpHeaders;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

@SuppressWarnings("unchecked")
public class RestUtil {


    private static final ObjectMapper mapper = new ObjectMapper();


    public static <T> T askItem(Class clazz,int port,String path,String verb,Object param) {

        try {

            Client client = ClientBuilder.newClient();
            WebTarget target = client.target("http://localhost:"+port).path(path);
            Invocation.Builder invocationBuilder = target.request(MediaType.APPLICATION_JSON);
            invocationBuilder.header(HttpHeaders.ACCEPT_ENCODING, MediaType.APPLICATION_JSON);

            String paramString = "";
            if(param!=null){
                paramString = mapper.writeValueAsString(param);
            }
            Response response = null;
            if(verb==null) verb = "get";
            String responseStr ="";
            if(verb.equalsIgnoreCase("post")) {
                response = invocationBuilder.post(Entity.json(paramString));
                responseStr = response.readEntity(String.class);
            }else if(verb.equalsIgnoreCase("delete")) {
                response = invocationBuilder.delete();
                responseStr = response.readEntity(String.class);
            }else if(verb.equalsIgnoreCase("put")) {
                response = invocationBuilder.put(Entity.json(paramString));
                responseStr = response.readEntity(String.class);
            }else{
                response = invocationBuilder.header("Content-type", "application/json").get();
                responseStr = response.readEntity(String.class);
            }
            if(clazz ==null) return null;
            T result = (T)mapper.readValue(responseStr,clazz);
            if(path.indexOf("command")>=0){
                Thread.sleep(1000);
            }
            return result;
        } catch (Exception ex) {
            return null;
        }
    }
}
