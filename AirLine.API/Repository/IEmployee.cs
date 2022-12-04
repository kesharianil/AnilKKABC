namespace AirLine.API.Repository
{
    public interface IEmployee
    {
        public Task<Employee> EmployeeList();
        public Task<Employee> AddEmployee(Employee employee);
    }
}
