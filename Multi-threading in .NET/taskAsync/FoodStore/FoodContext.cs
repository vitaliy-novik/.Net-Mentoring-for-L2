using System.Data.Entity;

namespace FoodStore
{
	class FoodContext : DbContext
	{
		public FoodContext() : base("DefaultConnection") { }

		public DbSet<Food> Foods { get; set; }
	}
}
