Feature: Users
	As a valid Platform user
	I want to request users list for CRUD operations
	So that I could see users list, filter it and make another operations

Background:
	Given the following userTypes exists in the storage
	| Id | Name          | AllowEdit |
	| 1  | Пользователь  | false     |
	| 2  | Администратор | true      |
	| 3  | Модератор     | true      |
	| 4  | Тестировщик   | false     |
	| 5  | Гость         | false     |
		And the following users exists in the storage
		| Id | TypeName      | Login       | Password   | Name      |
		| 1  | Администратор | Admin1Login | Admin1Pass | Admin     |
		| 2  | Гость         | Guest1Login | Guest1Pass | Guest     |
		| 3  | Пользователь  | User1Login  | User1Pass  | User      |
		| 4  | Модератор     | Mod1Login   | Mod1Pass   | Moderator |
		| 5  | Тестировщик   | Test1Login  | Test1Pass  | Tester    |

Scenario Outline: Get user with the specified identifier
	When  I request user belonging to <ids> Id
	Then the response Login is '<logins>'

	Examples:
	| ids | logins      |
	| 1   | Admin1Login |
	| 2   | Guest1Login |
	| 3   | User1Login  |
	| 4   | Mod1Login   |
	| 5   | Test1Login  |
