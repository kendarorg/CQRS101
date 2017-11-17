package unit;

import cucumber.api.junit.Cucumber;
import org.junit.Test;
import org.junit.runner.JUnitCore;
import org.junit.runner.RunWith;
import org.junit.runners.JUnit4;
import utils.DataUtils;

import static org.junit.Assert.assertEquals;

@RunWith(JUnit4.class)
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
