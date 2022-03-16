Feature: Authorization

Scenario: Users could sign in with valid credentials
	Given logged out user
	When the user attempt to sign in with valid credentials
	Then successfully signed in
