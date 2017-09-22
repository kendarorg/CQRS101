package org.cqrs101.external;

import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Named;
import org.cqrs.Bus;
import org.cqrs.MessageHandler;
import org.cqrs101.shared.tasks.TaskTitleVerified;

@Named("verifyTaskEventHandler")
public class VerifyTaskEventHandler implements MessageHandler {

    private Bus bus; 
    private static final Logger logger = Logger.getLogger(VerifyTaskEventHandler.class.getSimpleName());

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(m -> handle((VerifyTaskTitle) m), VerifyTaskTitle.class);
    }

    public void handle(VerifyTaskTitle command) {
        logger.log(Level.INFO, "{0}-VerifyTaskTitle", command.getCorrelationId());
        if (command.getTitle() != null && command.getTitle().length() > 0) {
            TaskTitleVerified message = new TaskTitleVerified();
            message.setId(command.getId());
            message.setCorrelationId(command.getCorrelationId());
            bus.send(message);
        }
    }

}
