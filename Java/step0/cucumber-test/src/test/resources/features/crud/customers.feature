Feature: Customers management
  Customers integration test

  Scenario: Create a user
    Given Data tables cleaned
    When A customer is inserted with name 'test1' and key '1'
    Then A customer with key '1' can be found

  Scenario: Delete a user
    Given Data tables cleaned
    And A customer is inserted with name 'test2' and key '2'
    And A customer with key '2' can be found
    When A customer with key '2' is deleted
    Then A customer with key '2' cannot be found
