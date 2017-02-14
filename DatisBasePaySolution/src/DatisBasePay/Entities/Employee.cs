using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DatisBasePay.Entities
{
	public class Employee
	{
		public int Id { get; set; }
		[Required, MaxLength(80)]
		public string EmployeeName { get; set; }
		[Required]
		public decimal Salary { get; set; }
		[Required]
		public decimal TaxRate { get; set; }
		[Required]
		public decimal Contribution { get; set; }
		[Required]
		public string StateOfResidence { get; set; }
	}
}
