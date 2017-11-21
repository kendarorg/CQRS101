Feature: Invoices basic management

  Scenario: Create an invoice
    Given Data tables cleaned
    And A customer is inserted with name 'test3' and key '3'
    When An invoice is created with customer '3' and id '3'
    Then An invoice is visible in InProgress with id '3'

  Scenario: Complete an invoice
    Given Data tables cleaned
    And A customer is inserted with name 'test4' and key '4'
    And An invoice is created with customer '4' and id '4'
    When The invoice with id '4'  is completed
    Then No invoices exists in InProgress with id '4'
    Then An invoice is visible in Completed with id '4'