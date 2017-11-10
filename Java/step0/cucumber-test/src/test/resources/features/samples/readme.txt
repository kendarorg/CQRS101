http://www.thinkcode.se/blog/2014/06/30/cucumber-data-tables
https://cucumber.io/docs/reference

Scenario: The sum of a list of numbers should be calculated
    Given a list of numbers
      | 17   |
      | 42   |
      | 4711 |
    When I summarize them
    Then should I get 4770




Scenario: A price list can be represented as price per item
    Given the price list for a coffee shop
      | coffee | 1 |
      | donut  | 2 |
    When I order 1 coffee and 1 donut
    Then should I pay 3




  Scenario: test Given 2
    Given Some given
    And A customer named '<data>' with id '<id>'
    When Some when
    Then Some then

  Examples:
  |data|id|
  |Test1|22|