package org.cqrs101.views.Services;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
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
import org.cqrs101.shared.Services.*;

@Named("tasksService")
public class TasksServiceApi implements TasksService {

    private ObjectMapper mapper = new ObjectMapper();
    @Override
    public TaskServiceDao GetById(UUID id) {
        try {
            Client client = ClientBuilder.newClient();
            WebTarget target = client.target("http://localhost:9000").path("api/tasks/"+id.toString());
            Invocation.Builder invocationBuilder = target.request(MediaType.APPLICATION_JSON);
            Response response = invocationBuilder.get();
            String responseStr = response.readEntity(String.class);
            return mapper.readValue(responseStr, TaskServiceDao.class);
        } catch (IOException ex) {
            Logger.getLogger(TasksServiceApi.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }
    }

    @Override
    public List<TaskServiceDao> GetAll() {
        
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

}
