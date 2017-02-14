using DatisBasePay.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatisBasePay.ViewModels
{
	public class EmployeeViewModel
	{
		public int Id { get; set; }
		[Required]
		[Display(Name = "Employee Name")]
		public string EmployeeName { get; set; }
		[Required]
		public decimal Salary { get; set; }
		[Required]
		[Display(Name = "Tax Rate")]
		public decimal TaxRate { get; set; }
		[Required]
		[Display(Name = "401k Contribution")]
		public decimal Contribution { get; set; }
		[Required]
		[Display(Name = "State of Residence")]
		public string StateOfResidence { get; set; }
		public IEnumerable<Employee> Employees { get; set; }
	}
}
