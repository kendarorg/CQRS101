package features.samples;

import cucumber.api.java.en.Given;
import cucumber.api.java.en.Then;
import cucumber.api.java.en.When;


public class SampleSteps {

    @Given("Some given")
    public void someGiven() {
        System.out.format("someGiven");
    }
    @When("Some when")
    public void someWhen() {
        System.out.format("someWhen");
    }
    @Then("Some then")
    public void someThen() {
        System.out.format("someThen");
    }
}
