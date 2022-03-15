@Authorization
Feature: Authorization
![Authorization tests](http://localhost/auth)
	In order to check sign in page

Scenario Outline: Users could sign in with valid credentials
	Given logged out user
	When the user attempt to sign in with valid credentials
	Then successfully signed in

	Examples:
	| endpoint |
	| auth     |
