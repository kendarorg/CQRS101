package org.tasks.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.io.IOException;
import java.util.List;
import javax.inject.Inject;
import org.cqrs.Bus;
import org.cqrs.Message;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/commands")
public class CommandsController {

    private final Bus _bus;
    private ObjectMapper mapper = new ObjectMapper();
    private final boolean initialized = false;

    @Inject
    public CommandsController(Bus bus) {
        _bus = bus;
    }

    @RequestMapping(
            value = "/types",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<String> GetTypes() {
        return _bus.getTypes();
    }

    @RequestMapping(
            value = "/send/{messageType}",
            method = RequestMethod.GET)
    public void SendMessage(@PathVariable("messageType") String messageType, @RequestBody String json) throws IOException {
        Class type = _bus.getType(messageType);

        Message message = (Message) mapper.readValue(json, type);
        _bus.Send(message);
    }
}
