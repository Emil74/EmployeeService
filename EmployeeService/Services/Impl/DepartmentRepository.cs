using EmployeeService.Data;
using EmployeeService.Models;

namespace EmployeeService.Services.Impl
{
    public class DepartmentRepository : IDepartmentRepository
    {

        #region service
        private readonly EmployeeServiceDbContext _context;

        public DepartmentRepository(EmployeeServiceDbContext context)
        {
            _context = context;
        }
        #endregion
        public int Create(Department data)
        {
            _context.Departments.Add(data);
            _context.SaveChanges();
            return data.Id;
        }

        public bool Delete(int id)
        {
            Department emp = GetById(id);
            if (emp != null)
            {
                _context.Departments.Remove(emp);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IList<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department GetById(int id)
        {
            return _context.Departments.FirstOrDefault(x => x.Id == id);
        }

        public bool Update(Department data)
        {
            Department emp = GetById(data.Id);
            if (emp != null)
            {
                emp.Description = data.Description;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
