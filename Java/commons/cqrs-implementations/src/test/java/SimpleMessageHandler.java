import org.cqrs.Bus;
import org.cqrs.MessageHandler;

import java.lang.reflect.ParameterizedType;

@SuppressWarnings("unchecked")
public abstract class SimpleMessageHandler<T> implements MessageHandler {
    @Override
    public void register(Bus bus) {
        Class<T> clazz = (Class<T>) ((ParameterizedType) getClass()
                .getGenericSuperclass()).getActualTypeArguments()[0];
        bus.registerHandler(m -> handle((T) m), clazz, this.getClass());
    }

    public abstract void handle(T message);
}
