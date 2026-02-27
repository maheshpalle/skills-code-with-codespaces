namespace EmployeeApp;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAll();
    Employee? GetById(int id);
    void Add(Employee employee);
    bool Update(Employee updated);
    bool Delete(int id);
}
