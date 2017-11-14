package utils;

public class DataUtils {
    public static String convertToUUID(int value){
        String base = "00000000-0000-0000-0000-000000000000";
        int baseLen = base.length();
        String valueStr = value+"";
        return base.substring(0,baseLen-valueStr.length())+valueStr;
    }
}
