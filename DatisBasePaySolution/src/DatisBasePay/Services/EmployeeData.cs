using DatisBasePay.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DatisBasePay.Services
{
	public interface IEmployeeData
	{
		IEnumerable<Employee> GetAll();
		Employee Get(int id);
		Employee Add(Employee employee);
		Employee Update(Employee employee);
		Employee Delete(Employee employee);
		void Commit();
	}

	public class SqlEmployeeData : IEmployeeData
	{
		private DatisBasePayDbContext _context;

		public SqlEmployeeData(DatisBasePayDbContext context)
		{
			_context = context;
		}

		public Employee Get(int id)
		{
			return _context.Employees.FirstOrDefault(e => e.Id == id);
		}

		public IEnumerable<Employee> GetAll()
		{
			return _context.Employees;
		}

		public Employee Add(Employee employee)
		{
			_context.Add(employee);
			return employee;
		}

		public Employee Update(Employee employee)
		{
			_context.Update(employee);
			return employee;
		}

		public Employee Delete(Employee employee)
		{
			_context.Remove(employee);
			return employee;
		}

		public void Commit()
		{
			_context.SaveChanges();
		}
	}
}
