package org.cqrs101.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.io.IOException;
import java.util.List;
import java.util.UUID;
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

    private final Bus bus;
    private final ObjectMapper mapper = new ObjectMapper();

    @Inject
    public CommandsController(Bus bus) {
        this.bus = bus;
    }

    @RequestMapping(
            value = "/types",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<String> getTypes() {
        return bus.getTypes();
    }

    @RequestMapping(
            value = "/send/{messageType}",
            method = RequestMethod.POST,
            produces = MediaType.APPLICATION_JSON_VALUE,
            consumes = MediaType.APPLICATION_JSON_VALUE)
    public UUID sendMessage(@PathVariable("messageType") String messageType, @RequestBody String json) throws IOException {
        Class type = bus.getType(messageType);

        Message message = (Message) mapper.readValue(json, type);
        message.setCorrelationId(UUID.randomUUID());
        bus.send(message);
        return message.getCorrelationId();
    }
}
