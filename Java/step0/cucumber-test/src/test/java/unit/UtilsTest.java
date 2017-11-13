package unit;

import org.junit.Test;
import utils.DataUtils;

import static org.junit.Assert.assertEquals;

public class UtilsTest {
    @Test
    public void shouldConvertIntToFakeUUID()
    {
        int start = 1;
        String expected = "00000000-0000-0000-0000-000000000001";
        String result = DataUtils.convertToUUID(start);
        assertEquals(expected,result);
    }
}
