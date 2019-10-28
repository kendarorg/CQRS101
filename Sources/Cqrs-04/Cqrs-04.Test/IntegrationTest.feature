Feature: IntegrationTest
	Should handle all cruise creation steps

Scenario: Create a cruise room
	When I add the cruise named 'Test'
	And  i assign a '2' class room with number '1' to the cruise named 'Test'
	Then i can get the cruise named 'Test' from projection
	And i can see '1' '2' class room for the cruise named 'Test'
