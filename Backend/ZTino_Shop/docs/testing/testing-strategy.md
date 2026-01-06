# Testing Strategy

This document explains the unit testing approach for ZTino_Shop backend.

## Overview

- **Framework**: xUnit
- **Focus**: Handler unit tests
- **Location**: `ZTino_Shop/tests/Application.Tests/`

## Test Structure

```
tests/Application.Tests/
├── Products/
│   └── v1/
│       ├── Commands/           # Command handler tests
│       │   ├── CreateProduct/
│       │   ├── UpdateProduct/
│       │   └── ...
│       └── Queries/            # Query handler tests
│           ├── GetProductById/
│           └── ...
├── Carts/
│   └── v1/
│       ├── Commands/
│       └── Queries/
├── Orders/
│   └── v1/
└── GlobalUsings.cs             # Common usings
```

---

## Test Organization

Tests mirror the Application layer structure:
- One test file per handler
- Named: `{HandlerName}Tests.cs`
- Location matches handler location

### Example

```
Application/Features/Products/v1/Commands/CreateProduct/
└── CreateProductHandler.cs

tests/Application.Tests/Products/v1/Commands/CreateProduct/
└── CreateProductHandlerTests.cs
```

---

## Test Categories

### Command Handler Tests

| Test Type | Purpose |
|-----------|---------|
| Success path | Handler completes correctly |
| Validation | Invalid input rejected |
| Not found | Missing resources handled |
| Business rules | Domain rules enforced |
| Side effects | Correct repository calls |

### Query Handler Tests

| Test Type | Purpose |
|-----------|---------|
| Returns data | Correct DTO returned |
| Filtering | Filters apply correctly |
| Not found | Returns null or empty |
| Mapping | Entity → DTO correct |

---

## Mocking

Tests use mocks for dependencies:
- Repository interfaces
- External services
- Current user

Popular mocking library: Moq (or similar)

---

## Test Naming Convention

```
MethodName_Scenario_ExpectedBehavior
```

Examples:
- `Handle_ValidProduct_ReturnsCreatedProduct`
- `Handle_ProductNotFound_ThrowsNotFoundException`
- `Handle_DuplicateSku_ThrowsConflictException`

---

## Test File Structure

```csharp
public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new CreateProductHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var command = new CreateProductCommand { ... };
        _productRepositoryMock.Setup(x => x.AddAsync(...));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        _productRepositoryMock.Verify(x => x.AddAsync(...), Times.Once);
    }
}
```

---

## Running Tests

```bash
# Run all tests
dotnet test

# Run specific project
dotnet test tests/Application.Tests

# Run with verbosity
dotnet test --logger "console;verbosity=detailed"

# Run specific test
dotnet test --filter "FullyQualifiedName~CreateProductHandler"
```

---

## Test Coverage

Tests focus on:
1. **Handlers**: All command and query handlers
2. **Validators**: Validation rule testing
3. **Domain logic**: Entity business methods

Not typically tested:
- Controllers (integration tests)
- EF Core configurations (migration tests)
- External services (mock in unit tests)

---

## Best Practices

1. **Test one thing**: Each test validates one behavior
2. **Arrange-Act-Assert**: Clear test structure
3. **Meaningful names**: Describe scenario and expectation
4. **Mock dependencies**: Isolate unit under test
5. **Test edge cases**: Nulls, empty collections, boundaries
6. **Keep tests fast**: No real database or network

---

## CI/CD Integration

Tests run automatically in pipeline:
1. Build project
2. Run `dotnet test`
3. Fail pipeline on test failures
4. Report coverage (optional)
