package controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.atomic.AtomicLong;
import org.cqrs.Bus;
import org.cqrs.Command;
import org.cqrs.Repository;
import org.springframework.beans.factory.support.BeanDefinitionRegistry;
import org.springframework.beans.factory.support.SimpleBeanDefinitionRegistry;
import org.springframework.context.annotation.ClassPathBeanDefinitionScanner;
import org.springframework.core.type.filter.AssignableTypeFilter;
import org.springframework.core.type.filter.TypeFilter;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;
import org.tasksmanager.Repositories.DoneTaskDao;

@RestController
@RequestMapping("/api/commands")
public class CommandsController {

    private Bus _bus;
    private ObjectMapper mapper = new ObjectMapper();
    private boolean initialized = false;
    

    public CommandsController(Bus bus) {
        _bus = bus;
        if(!initialized){
            initialized = true;
            BeanDefinitionRegistry bdr = new SimpleBeanDefinitionRegistry();
            ClassPathBeanDefinitionScanner s = new ClassPathBeanDefinitionScanner(bdr);

            TypeFilter tf = new AssignableTypeFilter(Command.class);
            s.addIncludeFilter(tf);
            s.scan("com");       
            String[] beans = bdr.getBeanDefinitionNames();
        }
    }

    @RequestMapping(
            value = "/send/{messageType}",
            method = RequestMethod.GET)
    public void GetById(@PathVariable("messageType") String messageType, @RequestBody String json) {
        //mapper.readValue(json, messageType);
        _bus.Send(null);
    }
}
