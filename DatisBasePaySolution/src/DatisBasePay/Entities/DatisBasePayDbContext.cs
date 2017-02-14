using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatisBasePay.Entities
{
	public class DatisBasePayDbContext : IdentityDbContext<User>
	{

		public DatisBasePayDbContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<Employee> Employees { get; set; }
	}
}
