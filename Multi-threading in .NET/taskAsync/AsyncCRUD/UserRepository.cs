using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AsyncCRUD
{
	public class UserRepository
	{
		private UserContext userContext;

		public UserRepository(string connectionString)
		{
			this.userContext = new UserContext(connectionString);
		}

		public async Task<User> Create(User user)
		{
			User result = this.userContext.Users.Add(user);
			await this.userContext.SaveChangesAsync();

			return result;
		}

		public async Task<IEnumerable<User>> Read(Expression<Func<User, bool>> predicate)
		{
			List<User> users = await this.userContext.Users
				.Where(predicate)
				.ToListAsync();

			return users;
		}

		public async Task<User> Update(User user)
		{
			User result = this.userContext.Users.Attach(user);
			this.userContext.Entry(user).State = EntityState.Modified;

			await this.userContext.SaveChangesAsync();

			return result;
		}

		public async Task<User> Delete(User user)
		{
			this.userContext.Users.Attach(user);
			User result = this.userContext.Users.Remove(user);

			await this.userContext.SaveChangesAsync();

			return user;
		}

		public async Task<IEnumerable<User>> Clear()
		{
			var users = this.userContext.Users.RemoveRange(this.userContext.Users);

			await this.userContext.SaveChangesAsync();

			return users;
		}
	}
}
