package org.cqrs101.services;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.cqrs101.shared.customers.CustomerDto;
import java.io.IOException;
import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Named;
import javax.ws.rs.client.Client;
import javax.ws.rs.client.ClientBuilder;
import javax.ws.rs.client.Invocation;
import javax.ws.rs.client.WebTarget;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import org.cqrs101.shared.customers.CustomersService;

@Named("usersService")
public class UsersServiceImpl implements CustomersService {

    private static final ObjectMapper mapper = new ObjectMapper();

    @Override
    public CustomerDto getCustomer(UUID id) {
        try {
            Client client = ClientBuilder.newClient();
            WebTarget target = client.target("http://localhost:9000").path("api/tasks/" + id.toString());
            Invocation.Builder invocationBuilder = target.request(MediaType.APPLICATION_JSON);
            Response response = invocationBuilder.get();
            String responseStr = response.readEntity(String.class);
            return mapper.readValue(responseStr, CustomerDto.class);
        } catch (IOException ex) {
            Logger.getLogger(UsersServiceImpl.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }
    }
    /*
    @Override
    public List<TaskServiceDao> getAll() {
        
        try {
            Client client = ClientBuilder.newClient();
            WebTarget target = client.target("http://localhost:9001").path("api/tasks");
            Invocation.Builder invocationBuilder = target.request(MediaType.APPLICATION_JSON);
            Response response = invocationBuilder.get();
            String responseStr = response.readEntity(String.class);
            return mapper.readValue(responseStr, new TypeReference<List<TaskServiceDao>>(){});
        } catch (IOException ex) {
            Logger.getLogger(TasksServiceApi.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }
        
    }
    */
}
