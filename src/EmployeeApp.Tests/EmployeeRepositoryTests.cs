using EmployeeApp;

namespace EmployeeApp.Tests;

public class EmployeeRepositoryTests
{
    private EmployeeRepository CreateRepository() => new EmployeeRepository();

    // GetAll tests
    [Fact]
    public void GetAll_EmptyRepository_ReturnsEmptyCollection()
    {
        var repo = CreateRepository();
        Assert.Empty(repo.GetAll());
    }

    [Fact]
    public void GetAll_AfterAddingEmployees_ReturnsAllEmployees()
    {
        var repo = CreateRepository();
        repo.Add(new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 });
        repo.Add(new Employee { Name = "Bob", Department = "Marketing", Salary = 70000 });

        var employees = repo.GetAll().ToList();

        Assert.Equal(2, employees.Count);
    }

    // GetById tests
    [Fact]
    public void GetById_ExistingId_ReturnsCorrectEmployee()
    {
        var repo = CreateRepository();
        repo.Add(new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 });

        var emp = repo.GetById(1);

        Assert.NotNull(emp);
        Assert.Equal("Alice", emp.Name);
        Assert.Equal("Engineering", emp.Department);
        Assert.Equal(90000, emp.Salary);
    }

    [Fact]
    public void GetById_NonExistingId_ReturnsNull()
    {
        var repo = CreateRepository();

        var emp = repo.GetById(999);

        Assert.Null(emp);
    }

    // Add tests
    [Fact]
    public void Add_NewEmployee_AssignsIncrementingId()
    {
        var repo = CreateRepository();
        var emp1 = new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 };
        var emp2 = new Employee { Name = "Bob", Department = "Marketing", Salary = 70000 };

        repo.Add(emp1);
        repo.Add(emp2);

        Assert.Equal(1, emp1.Id);
        Assert.Equal(2, emp2.Id);
    }

    [Fact]
    public void Add_NewEmployee_CanBeRetrievedById()
    {
        var repo = CreateRepository();
        var emp = new Employee { Name = "Carol", Department = "HR", Salary = 65000 };

        repo.Add(emp);

        var retrieved = repo.GetById(emp.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Carol", retrieved.Name);
    }

    [Fact]
    public void Add_MultipleEmployees_AllStoredCorrectly()
    {
        var repo = CreateRepository();
        repo.Add(new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 });
        repo.Add(new Employee { Name = "Bob", Department = "Marketing", Salary = 70000 });
        repo.Add(new Employee { Name = "Carol", Department = "HR", Salary = 65000 });

        Assert.Equal(3, repo.GetAll().Count());
    }

    // Update tests
    [Fact]
    public void Update_ExistingEmployee_ReturnsTrueAndUpdatesFields()
    {
        var repo = CreateRepository();
        repo.Add(new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 });

        var result = repo.Update(new Employee { Id = 1, Name = "Alice Smith", Department = "Finance", Salary = 95000 });

        Assert.True(result);
        var emp = repo.GetById(1);
        Assert.NotNull(emp);
        Assert.Equal("Alice Smith", emp.Name);
        Assert.Equal("Finance", emp.Department);
        Assert.Equal(95000, emp.Salary);
    }

    [Fact]
    public void Update_NonExistingEmployee_ReturnsFalse()
    {
        var repo = CreateRepository();

        var result = repo.Update(new Employee { Id = 999, Name = "Ghost", Department = "Unknown", Salary = 0 });

        Assert.False(result);
    }

    [Fact]
    public void Update_DoesNotAffectOtherEmployees()
    {
        var repo = CreateRepository();
        repo.Add(new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 });
        repo.Add(new Employee { Name = "Bob", Department = "Marketing", Salary = 70000 });

        repo.Update(new Employee { Id = 1, Name = "Alice Updated", Department = "Finance", Salary = 95000 });

        var bob = repo.GetById(2);
        Assert.NotNull(bob);
        Assert.Equal("Bob", bob.Name);
        Assert.Equal("Marketing", bob.Department);
    }

    // Delete tests
    [Fact]
    public void Delete_ExistingEmployee_ReturnsTrueAndRemovesEmployee()
    {
        var repo = CreateRepository();
        repo.Add(new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 });

        var result = repo.Delete(1);

        Assert.True(result);
        Assert.Null(repo.GetById(1));
    }

    [Fact]
    public void Delete_NonExistingEmployee_ReturnsFalse()
    {
        var repo = CreateRepository();

        var result = repo.Delete(999);

        Assert.False(result);
    }

    [Fact]
    public void Delete_OneEmployee_OthersRemainIntact()
    {
        var repo = CreateRepository();
        repo.Add(new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 });
        repo.Add(new Employee { Name = "Bob", Department = "Marketing", Salary = 70000 });

        repo.Delete(1);

        Assert.Single(repo.GetAll());
        Assert.NotNull(repo.GetById(2));
    }

    [Fact]
    public void Delete_AllEmployees_RepositoryIsEmpty()
    {
        var repo = CreateRepository();
        repo.Add(new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 });
        repo.Add(new Employee { Name = "Bob", Department = "Marketing", Salary = 70000 });

        repo.Delete(1);
        repo.Delete(2);

        Assert.Empty(repo.GetAll());
    }

    // Interface compliance test
    [Fact]
    public void EmployeeRepository_ImplementsIEmployeeRepository()
    {
        var repo = CreateRepository();
        Assert.IsAssignableFrom<IEmployeeRepository>(repo);
    }

    // Id sequencing after delete
    [Fact]
    public void Add_AfterDelete_ContinuesIncrementingId()
    {
        var repo = CreateRepository();
        repo.Add(new Employee { Name = "Alice", Department = "Engineering", Salary = 90000 });
        repo.Delete(1);
        var emp = new Employee { Name = "Dave", Department = "Sales", Salary = 60000 };

        repo.Add(emp);

        Assert.Equal(2, emp.Id);
    }
}
