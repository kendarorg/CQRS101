package features.commons;

import cucumber.api.PendingException;
import cucumber.api.java.en.Given;
import cucumber.api.java.en.Then;
import cucumber.api.java.en.When;

public class CustomersSteps {
    @When("^A user is inserted with name '(.*)' and key '(\\d+)'$")
    public void a_user_is_inserted_with_name_test_and_key(String arg1, int arg2) throws Exception {
        // Write code here that turns the phrase above into concrete actions
    }

    @Then("^A user with key '(\\d+)' can be found$")
    public void a_user_with_key_can_be_found(int arg1) throws Exception {
        // Write code here that turns the phrase above into concrete actions
    }

    @When("^A user with key '(\\d+)' is deleted$")
    public void a_user_with_key_is_deleted(int arg1) throws Exception {
        // Write code here that turns the phrase above into concrete actions
    }

    @Then("^A user with key '(\\d+)' cannot be found$")
    public void a_user_with_key_cannot_be_found(int arg1) throws Exception {
        // Write code here that turns the phrase above into concrete actions
    }
}
