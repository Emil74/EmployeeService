using EmployeeService.Data;
using EmployeeService.Models;

namespace EmployeeService.Services.Impl
{
    public class EmployeeRepository : IEmployeeRepository
    {
        #region service
        private readonly EmployeeServiceDbContext _context;

        #endregion

        public EmployeeRepository(EmployeeServiceDbContext context)
        {
            _context = context;
        }
        public int Create(Employee data)
        {
            _context.Employees.Add(data);
            _context.SaveChanges();
            return data.Id;
        }

        public bool Delete(int id)
        {
            Employee emp = GetById(id);
            if (emp!=null)
            {
                _context.Employees.Remove(emp);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IList<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.FirstOrDefault(x => x.Id == id);
        }

        public bool Update(Employee data)
        {
            Employee emp = GetById(data.Id);
            if (emp!=null)
            {
                emp.DepartmentId = data.DepartmentId;
                emp.EmployeeTypeId = data.EmployeeTypeId;
                emp.FirstName = data.FirstName;
                emp.Surname = data.Surname;
                emp.Patronymic = data.Patronymic;
                emp.Salary = data.Salary;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
