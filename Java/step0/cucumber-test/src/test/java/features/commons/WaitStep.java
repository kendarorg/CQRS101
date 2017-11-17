package features.commons;

import cucumber.api.java.en.When;

public class WaitStep {
    @When("^'(\\d+)' ms has gone$")
    public void ms_has_gone(int ms) throws Exception {
        Thread.sleep(ms);
    }
}
