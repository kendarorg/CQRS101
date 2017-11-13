Feature: Customers management
  Customers integration test

  Scenario: Create a user
    When A user is inserted with name 'test1' and key '1'
    Then A user with key '1' can be found

  Scenario: Delete a user
    Given A user is inserted with name 'test2' and key '2'
    And A user with key '1' can be found
    When A user with key '2' is deleted
    Then A user with key '2' cannot be found
