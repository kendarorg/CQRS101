package features.invoices;

import cucumber.api.java.en.Then;
import cucumber.api.java.en.When;
import org.cqrs101.invoices.commands.CompleteInvoice;
import org.cqrs101.invoices.commands.CreateInvoice;
import org.cqrs101.shared.customers.CustomerDto;
import org.cqrs101.views.repositories.CompletedInvoice;
import org.cqrs101.views.repositories.InProgressInvoice;
import utils.DataUtils;
import utils.RestUtil;
import utils.WaiterUtil;

import java.util.UUID;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;
import static org.junit.Assert.assertNull;

public class InvoicesBasicSteps {
    @When("^An invoice is created with customer '(\\d+)' and id '(\\d+)'$")
    public void an_invoice_is_created_with_user_and_id(int customerIdInt, int invoiceIdInt) throws Exception {
        UUID customerId = UUID.fromString(DataUtils.convertToUUID(customerIdInt));
        UUID invoiceId = UUID.fromString(DataUtils.convertToUUID(invoiceIdInt));

        CreateInvoice command = new CreateInvoice();
        command.setCustomerId(customerId);
        command.setId(invoiceId);
        RestUtil.askItem(null,8090,"/api/commands/send/CreateInvoice",
                "POST",command);
    }

    @Then("^An invoice is visible in InProgress with id '(\\d+)'$")
    public void the_invoice_is_visible_in_InProgress_with_id(int invoiceIdInt) throws Exception {
        UUID invoiceId = UUID.fromString(DataUtils.convertToUUID(invoiceIdInt));
        InProgressInvoice result = WaiterUtil.waitTimeout(()->
                RestUtil.askItem(InProgressInvoice.class,8092,"api/invoices/inprogress/"+invoiceId.toString(),"GET",null));
        assertEquals(invoiceId,result.getId());
    }

    @When("^The invoice with id '(\\d+)'  is completed$")
    public void the_invoice_with_id_is_completed(int invoiceIdInt) throws Exception {
        UUID invoiceId = UUID.fromString(DataUtils.convertToUUID(invoiceIdInt));

        CompleteInvoice command = new CompleteInvoice();
        command.setId(invoiceId);
        RestUtil.askItem(null,8090,"/api/commands/send/CompleteInvoice",
                "POST",command);
    }

    @Then("^No invoices exists in InProgress with id '(\\d+)'$")
    public void no_invoices_exists_in_InProgress_with_id(int invoiceIdInt) throws Exception {
        UUID invoiceId = UUID.fromString(DataUtils.convertToUUID(invoiceIdInt));
        InProgressInvoice result = RestUtil.askItem(InProgressInvoice.class,8092,"/api/invoices/inprogress/"+invoiceId.toString(),"GET",null);
        assertNull(result);
    }

    @Then("^An invoice is visible in Completed with id '(\\d+)'$")
    public void an_invoice_is_visible_in_Completed_with_id(int invoiceIdInt) throws Exception {
        UUID invoiceId = UUID.fromString(DataUtils.convertToUUID(invoiceIdInt));
        CompletedInvoice result = WaiterUtil.waitTimeout(()->
                RestUtil.askItem(CompletedInvoice.class,8092,"/api/invoices/completed/"+invoiceId.toString(),"GET",null));
        assertNotNull(result);
        assertEquals(invoiceId,result.getId());
    }
}
