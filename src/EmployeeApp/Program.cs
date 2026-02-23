using EmployeeApp;

var repository = new EmployeeRepository();

// Seed some initial data
repository.Add(new Employee { Name = "Alice Johnson", Department = "Engineering", Salary = 95000 });
repository.Add(new Employee { Name = "Bob Smith", Department = "Marketing", Salary = 72000 });
repository.Add(new Employee { Name = "Carol White", Department = "HR", Salary = 68000 });

while (true)
{
    Console.WriteLine("\n=== Employee CRUD App ===");
    Console.WriteLine("1. List all employees");
    Console.WriteLine("2. Get employee by ID");
    Console.WriteLine("3. Add employee");
    Console.WriteLine("4. Update employee");
    Console.WriteLine("5. Delete employee");
    Console.WriteLine("6. Exit");
    Console.Write("Choose an option: ");

    var input = Console.ReadLine()?.Trim();

    switch (input)
    {
        case "1":
            ListAll(repository);
            break;
        case "2":
            GetById(repository);
            break;
        case "3":
            AddEmployee(repository);
            break;
        case "4":
            UpdateEmployee(repository);
            break;
        case "5":
            DeleteEmployee(repository);
            break;
        case "6":
            Console.WriteLine("Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}

static void ListAll(EmployeeRepository repo)
{
    var employees = repo.GetAll();
    if (!employees.Any())
    {
        Console.WriteLine("No employees found.");
        return;
    }
    Console.WriteLine($"\n{"ID",-5} {"Name",-25} {"Department",-20} {"Salary",10}");
    Console.WriteLine(new string('-', 62));
    foreach (var emp in employees)
    {
        Console.WriteLine($"{emp.Id,-5} {emp.Name,-25} {emp.Department,-20} {emp.Salary,10:C0}");
    }
}

static void GetById(EmployeeRepository repo)
{
    Console.Write("Enter employee ID: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }
    var emp = repo.GetById(id);
    if (emp == null)
    {
        Console.WriteLine($"Employee with ID {id} not found.");
        return;
    }
    Console.WriteLine($"\nID: {emp.Id}");
    Console.WriteLine($"Name: {emp.Name}");
    Console.WriteLine($"Department: {emp.Department}");
    Console.WriteLine($"Salary: {emp.Salary:C0}");
}

static void AddEmployee(EmployeeRepository repo)
{
    Console.Write("Name: ");
    var name = Console.ReadLine()?.Trim() ?? string.Empty;
    if (string.IsNullOrEmpty(name))
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    Console.Write("Department: ");
    var department = Console.ReadLine()?.Trim() ?? string.Empty;
    if (string.IsNullOrEmpty(department))
    {
        Console.WriteLine("Department cannot be empty.");
        return;
    }

    Console.Write("Salary: ");
    if (!decimal.TryParse(Console.ReadLine(), out var salary))
    {
        Console.WriteLine("Invalid salary.");
        return;
    }

    var emp = new Employee { Name = name, Department = department, Salary = salary };
    repo.Add(emp);
    Console.WriteLine($"Employee added with ID {emp.Id}.");
}

static void UpdateEmployee(EmployeeRepository repo)
{
    Console.Write("Enter employee ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }
    var emp = repo.GetById(id);
    if (emp == null)
    {
        Console.WriteLine($"Employee with ID {id} not found.");
        return;
    }

    Console.Write($"Name [{emp.Name}]: ");
    var name = Console.ReadLine()?.Trim();
    if (!string.IsNullOrEmpty(name)) emp.Name = name;

    Console.Write($"Department [{emp.Department}]: ");
    var department = Console.ReadLine()?.Trim();
    if (!string.IsNullOrEmpty(department)) emp.Department = department;

    Console.Write($"Salary [{emp.Salary:C0}]: ");
    var salaryInput = Console.ReadLine()?.Trim();
    if (!string.IsNullOrEmpty(salaryInput) && decimal.TryParse(salaryInput, out var salary))
        emp.Salary = salary;

    repo.Update(emp);
    Console.WriteLine("Employee updated successfully.");
}

static void DeleteEmployee(EmployeeRepository repo)
{
    Console.Write("Enter employee ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }
    if (repo.Delete(id))
        Console.WriteLine($"Employee with ID {id} deleted.");
    else
        Console.WriteLine($"Employee with ID {id} not found.");
}
