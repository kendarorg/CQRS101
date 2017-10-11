package org.cqrs101.services;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.util.UUID;
import javax.inject.Named;
import javax.ws.rs.client.*;
import javax.ws.rs.core.Response;
import org.cqrs101.shared.users.UserDto;
import org.cqrs101.shared.users.UsersService;
import java.io.IOException;
import java.util.List;
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

@Named("usersService")
public class UsersServiceImpl implements UsersService {

    private static final ObjectMapper mapper = new ObjectMapper();

    @Override
    public UserDto getUser(UUID id) {
        try {
            Client client = ClientBuilder.newClient();
            WebTarget target = client.target("http://localhost:9000").path("api/tasks/" + id.toString());
            Invocation.Builder invocationBuilder = target.request(MediaType.APPLICATION_JSON);
            Response response = invocationBuilder.get();
            String responseStr = response.readEntity(String.class);
            return mapper.readValue(responseStr, UserDto.class);
        } catch (IOException ex) {
            Logger.getLogger(UsersServiceImpl.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }
    }
}
