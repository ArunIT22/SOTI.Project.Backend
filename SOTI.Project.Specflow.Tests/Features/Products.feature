Feature: Products
	As a user
	I want to perform view, add, update products

@product
Scenario: View Products
	Given I have a mocked product controllter
	When I sent a GET Request to endpoint
	Then the response should contain list of products
	And the response should be OK result
