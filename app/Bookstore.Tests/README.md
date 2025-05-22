# Bookstore Unit Tests

This project contains unit tests for the Bob's Used Bookstore Classic application.

## Test Structure

The tests are organized by project and component:

- **Helpers/**: Tests for extension methods and helper classes
- **Domain/**: Tests for domain entities and business logic
- **Data/**: Tests for data access and repository classes
- **Web/**: Tests for web controllers and components

## Running Tests

To run the tests, use the following command:

```bash
dotnet test
```

## Test Patterns

The tests follow these patterns:

1. **Arrange**: Set up the test data and environment
2. **Act**: Execute the method being tested
3. **Assert**: Verify the results

## Mocking

The tests use Moq for mocking dependencies. This allows testing components in isolation.

## Adding New Tests

When adding new tests:

1. Place them in the appropriate folder based on what they're testing
2. Follow the naming convention: `{ClassBeingTested}Tests.cs`
3. Use descriptive test method names that explain what is being tested
4. Use the AAA pattern (Arrange, Act, Assert)
5. Keep tests focused on a single behavior