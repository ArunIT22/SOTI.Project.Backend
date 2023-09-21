Feature: Products
	As a user
	I want to perform view, add, update products

@product
Scenario: View Products
	Given I have a mocked product controllter
	When I sent a GET Request to endpoint
	Then the response should contain list of products
	And the response should be OK result

Scenario: Add New Product
	Given I have a mocked the ProductDetails for insert
	And I have a product to insert
	When I send a POST request to api/products/addproduct endpoint
	Then the response status should be 201
	And the response should contain the inserted product

Scenario: Update an existing Product
	Given I have a mocked the ProductDetails for Update
	And I have an existing product
	When I send a PUT Request to endpoint
	Then the response status code should be 204
	And the response should contain the update the product