Feature: Invoices basic management

  Scenario: Create an invoice
    Given A user is inserted with name 'test1' and key '1'
    When An invoice is created with user '1' and id '2'
    And '1000' ms has gone
    Then An invoice is visible in InProgress with id '2'

  Scenario: Complete an invoice
    Given A user is inserted with name 'test2' and key '2'
    And An invoice is created with user '2' and id '3'
    And '1000' ms has gone
    When The invoice with id '3'  is completed
    And '1000' ms has gone
    Then No invoices exists in InProgress with id '3'
    Then An invoice is visible in Completed with id '3'