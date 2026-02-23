namespace EmployeeApp;

public class EmployeeRepository
{
    private readonly List<Employee> _employees = new();
    private int _nextId = 1;

    public IEnumerable<Employee> GetAll() => _employees.AsReadOnly();

    public Employee? GetById(int id) =>
        _employees.FirstOrDefault(e => e.Id == id);

    public void Add(Employee employee)
    {
        employee.Id = _nextId++;
        _employees.Add(employee);
    }

    public bool Update(Employee updated)
    {
        var existing = GetById(updated.Id);
        if (existing == null) return false;

        existing.Name = updated.Name;
        existing.Department = updated.Department;
        existing.Salary = updated.Salary;
        return true;
    }

    public bool Delete(int id)
    {
        var employee = GetById(id);
        if (employee == null) return false;

        _employees.Remove(employee);
        return true;
    }
}
