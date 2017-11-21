package org.cqrs101.services;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.cqrs101.shared.customers.CustomerDto;
import org.cqrs101.shared.customers.CustomersService;

import javax.inject.Named;
import javax.ws.rs.client.Client;
import javax.ws.rs.client.ClientBuilder;
import javax.ws.rs.client.Invocation;
import javax.ws.rs.client.WebTarget;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import java.io.IOException;
import java.util.List;
import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;

@Named("customersService")
public class CustomersServiceImpl implements CustomersService {

    private static long crudApiPort = 8091;
    private static final ObjectMapper mapper = new ObjectMapper();

    @Override
    public CustomerDto getCustomer(UUID id) {
        try {
            Client client = ClientBuilder.newClient();
            WebTarget target = client.target("http://localhost:"+crudApiPort).path("api/customers/" + id.toString());
            Invocation.Builder invocationBuilder = target.request(MediaType.APPLICATION_JSON);
            Response response = invocationBuilder.header("Content-type", "application/json").get();
            String responseStr = response.readEntity(String.class);
            if(responseStr==null || responseStr.length()==0){
                return null;
            }
            return mapper.readValue(responseStr, CustomerDto.class);
        } catch (IOException ex) {
            Logger.getLogger(CustomersServiceImpl.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }
    }

    @Override
    public List<CustomerDto> getAll() {
        
        try {
            Client client = ClientBuilder.newClient();
            WebTarget target = client.target("http://localhost:"+crudApiPort).path("api/customers");
            Invocation.Builder invocationBuilder = target.request(MediaType.APPLICATION_JSON);
            Response response = invocationBuilder.header("Content-type", "application/json").get();
            String responseStr = response.readEntity(String.class);
            if(responseStr==null || responseStr.length()==0){
                return null;
            }
            return mapper.readValue(responseStr, new TypeReference<List<CustomerDto>>(){});
        } catch (IOException ex) {
            Logger.getLogger(CustomersService.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }
        
    }

}
