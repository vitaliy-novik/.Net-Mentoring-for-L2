using System.Data.Entity;

namespace AsyncCRUD
{
	class UserContext : DbContext
	{
		public UserContext(string connectionString) : base(connectionString) { }

		public DbSet<User> Users { get; set; }
	}
}
