package features.commons;

//https://cucumber.io/docs/reference/jvm
//http://www.hascode.com/2014/12/bdd-testing-with-cucumber-java-and-junit/

import cucumber.api.CucumberOptions;
import cucumber.api.junit.Cucumber;
import org.junit.runner.RunWith;

@RunWith(Cucumber.class)
@CucumberOptions(
        plugin = {"pretty", "html:target/cucumber"},
        glue = {"features.commons"})
public class CustomersTest {
}
