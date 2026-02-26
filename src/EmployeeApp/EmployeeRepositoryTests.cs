using Xunit;

namespace EmployeeApp.Tests;

public class EmployeeRepositoryTests
{
    private readonly EmployeeRepository _repository = new();

    #region GetAll Tests
    [Fact]
    public void GetAll_WithNoEmployees_ReturnsEmptyCollection()
    {
        // Act
        var result = _repository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_WithEmployees_ReturnsAllEmployees()
    {
        // Arrange
        var employee1 = new Employee { Name = "John Doe", Department = "Engineering", Salary = 80000 };
        var employee2 = new Employee { Name = "Jane Smith", Department = "Sales", Salary = 70000 };
        _repository.Add(employee1);
        _repository.Add(employee2);

        // Act
        var result = _repository.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, e => e.Name == "John Doe");
        Assert.Contains(result, e => e.Name == "Jane Smith");
    }
    #endregion

    #region GetById Tests
    [Fact]
    public void GetById_WithValidId_ReturnsEmployee()
    {
        // Arrange
        var employee = new Employee { Name = "John Doe", Department = "Engineering", Salary = 80000 };
        _repository.Add(employee);
        var employeeId = _repository.GetAll().First().Id;

        // Act
        var result = _repository.GetById(employeeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("Engineering", result.Department);
        Assert.Equal(80000, result.Salary);
    }

    [Fact]
    public void GetById_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = _repository.GetById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetById_WithNegativeId_ReturnsNull()
    {
        // Act
        var result = _repository.GetById(-1);

        // Assert
        Assert.Null(result);
    }
    #endregion

    #region Add Tests
    [Fact]
    public void Add_WithValidEmployee_AssignsIdAndAddsEmployee()
    {
        // Arrange
        var employee = new Employee { Name = "John Doe", Department = "Engineering", Salary = 80000 };

        // Act
        _repository.Add(employee);

        // Assert
        Assert.NotEqual(0, employee.Id);
        Assert.Single(_repository.GetAll());
        Assert.Equal("John Doe", _repository.GetById(employee.Id)?.Name);
    }

    [Fact]
    public void Add_MultipleEmployees_AssignsIncrementingIds()
    {
        // Arrange
        var employee1 = new Employee { Name = "John Doe", Department = "Engineering", Salary = 80000 };
        var employee2 = new Employee { Name = "Jane Smith", Department = "Sales", Salary = 70000 };

        // Act
        _repository.Add(employee1);
        _repository.Add(employee2);

        // Assert
        Assert.Equal(1, employee1.Id);
        Assert.Equal(2, employee2.Id);
    }

    [Fact]
    public void Add_EmptyEmployee_AddsSuccessfully()
    {
        // Arrange
        var employee = new Employee();

        // Act
        _repository.Add(employee);

        // Assert
        Assert.Equal(1, employee.Id);
        Assert.Single(_repository.GetAll());
    }
    #endregion

    #region Update Tests
    [Fact]
    public void Update_WithValidEmployee_UpdatesProperties()
    {
        // Arrange
        var employee = new Employee { Name = "John Doe", Department = "Engineering", Salary = 80000 };
        _repository.Add(employee);
        var employeeId = employee.Id;

        var updatedEmployee = new Employee 
        { 
            Id = employeeId,
            Name = "John Smith", 
            Department = "Management", 
            Salary = 95000 
        };

        // Act
        var result = _repository.Update(updatedEmployee);

        // Assert
        Assert.True(result);
        var retrieved = _repository.GetById(employeeId);
        Assert.NotNull(retrieved);
        Assert.Equal("John Smith", retrieved.Name);
        Assert.Equal("Management", retrieved.Department);
        Assert.Equal(95000, retrieved.Salary);
    }

    [Fact]
    public void Update_WithNonexistentId_ReturnsFalse()
    {
        // Arrange
        var employee = new Employee 
        { 
            Id = 999,
            Name = "John Doe", 
            Department = "Engineering", 
            Salary = 80000 
        };

        // Act
        var result = _repository.Update(employee);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Update_DoesNotChangeId()
    {
        // Arrange
        var employee = new Employee { Name = "John Doe", Department = "Engineering", Salary = 80000 };
        _repository.Add(employee);
        var originalId = employee.Id;

        var updatedEmployee = new Employee 
        { 
            Id = originalId,
            Name = "Jane Doe", 
            Department = "Sales", 
            Salary = 75000 
        };

        // Act
        _repository.Update(updatedEmployee);

        // Assert
        var retrieved = _repository.GetById(originalId);
        Assert.NotNull(retrieved);
        Assert.Equal(originalId, retrieved.Id);
    }
    #endregion

    #region Delete Tests
    [Fact]
    public void Delete_WithValidId_RemovesEmployee()
    {
        // Arrange
        var employee = new Employee { Name = "John Doe", Department = "Engineering", Salary = 80000 };
        _repository.Add(employee);
        var employeeId = employee.Id;

        // Act
        var result = _repository.Delete(employeeId);

        // Assert
        Assert.True(result);
        Assert.Null(_repository.GetById(employeeId));
        Assert.Empty(_repository.GetAll());
    }

    [Fact]
    public void Delete_WithInvalidId_ReturnsFalse()
    {
        // Act
        var result = _repository.Delete(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Delete_RemovesOnlySpecificEmployee()
    {
        // Arrange
        var employee1 = new Employee { Name = "John Doe", Department = "Engineering", Salary = 80000 };
        var employee2 = new Employee { Name = "Jane Smith", Department = "Sales", Salary = 70000 };
        _repository.Add(employee1);
        _repository.Add(employee2);

        // Act
        var result = _repository.Delete(employee1.Id);

        // Assert
        Assert.True(result);
        Assert.Null(_repository.GetById(employee1.Id));
        Assert.NotNull(_repository.GetById(employee2.Id));
        Assert.Single(_repository.GetAll());
    }
    #endregion
}
