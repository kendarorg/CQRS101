package utils;
import java.util.function.Supplier;

public class WaiterUtil {

    public static <T> T waitTimeout(Supplier<T> towait){
        return waitTimeout(towait,5000);
    }

    public static <T> T waitTimeout(Supplier<T> towait,long timeout){
        long times = timeout/100;
        while(timeout>0){
            try{
                T result = towait.get();
                if(result!=null) {
                    return result;
                }
            }catch(Exception ex){

            }
            timeout-=times;
            try {
                Thread.sleep(times);
            }catch(Exception ex){

            }
        }
        return null;
    }
}
