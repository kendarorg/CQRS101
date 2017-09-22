package org.cqrs101.external;

import javax.inject.Named;
import org.cqrs.Bus;
import org.cqrs.MessageHandler;
import org.cqrs101.shared.Tasks.TaskTitleVerified;

@Named("verifyTaskEventHandler")
public class VerifyTaskEventHandler implements MessageHandler {

    private Bus _bus;

    @Override
    public void Register(Bus bus) {
        _bus = bus;
        _bus.RegisterHandler(m -> Handle((VerifyTaskTitle) m), VerifyTaskTitle.class);
    }

    public void Handle(VerifyTaskTitle verifyTaskTitleExt) {
        if (verifyTaskTitleExt.getTitle() != null && verifyTaskTitleExt.getTitle().length() > 0) {
            TaskTitleVerified message = new TaskTitleVerified();
            message.setId(verifyTaskTitleExt.getId());
            _bus.Send(message);
        }
    }

}
