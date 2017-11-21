package features.crud;

import cucumber.api.java.en.Given;
import cucumber.api.java.en.Then;
import cucumber.api.java.en.When;
import org.cqrs101.shared.customers.CustomerDto;
import utils.DataUtils;
import utils.RestUtil;
import utils.WaiterUtil;

import java.util.List;
import java.util.UUID;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.assertNull;

public class CustomersSteps {
    @When("^A customer is inserted with name '(.*)' and key '(\\d+)'$")
    //@Given("^A user is inserted with name '(.*)' and key '(\\d+)'$")
    public void a_user_is_inserted_with_name_test_and_key(String name, int id) throws Exception {
        UUID customerId = UUID.fromString(DataUtils.convertToUUID(id));
        CustomerDto newCustomer = new CustomerDto();
        newCustomer.setName(name);
        newCustomer.setId(customerId);
        CustomerDto result = WaiterUtil.waitTimeout(()->
                RestUtil.askItem(CustomerDto.class,8091,"/api/customers","POST",newCustomer));
        assertNotNull(result);
        assertEquals(newCustomer.getId(), result.getId());
    }

    @Then("^A customer with key '(\\d+)' can be found$")
    public void a_user_with_key_can_be_found(int id) throws Exception {
        UUID customerId = UUID.fromString(DataUtils.convertToUUID(id));
        CustomerDto result = WaiterUtil.waitTimeout(()->
                RestUtil.askItem(CustomerDto.class,8091,"/api/customers/"+customerId.toString(),"GET",null));
        assertEquals(customerId,result.getId());
    }

    @When("^A customer with key '(\\d+)' is deleted$")
    public void a_user_with_key_is_deleted(int id) throws Exception {
        UUID customerId = UUID.fromString(DataUtils.convertToUUID(id));
        RestUtil.askItem(CustomerDto.class,8091,"/api/customers/"+customerId.toString(),"DELETE",null);
    }

    @Then("^A customer with key '(\\d+)' cannot be found$")
    public void a_user_with_key_cannot_be_found(int id) throws Exception {
        UUID customerId = UUID.fromString(DataUtils.convertToUUID(id));
        CustomerDto result = RestUtil.askItem(CustomerDto.class,8091,"/api/customers/"+customerId.toString(),"GET",null);
        assertNull(result);
    }
}
