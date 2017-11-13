package features.samples2;

import cucumber.api.java.en.Given;

public class CrudSteps {
    @Given("A customer named '(.*)' with id '(\\d+)'")
    public void someGiven(String user,int id) {
        String def = "00000000-0000-0000-0000-000000000000";

        String sid = ""+id;
        int len = def.length()-sid.length();
        String newId = def.substring(0,len)+sid;
        System.out.format(newId+user);
    }
}
