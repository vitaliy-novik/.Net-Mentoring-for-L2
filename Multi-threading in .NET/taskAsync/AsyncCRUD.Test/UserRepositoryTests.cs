using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AsyncCRUD.Test
{
	[TestClass]
	public class UserRepositoryTests
	{
		UserRepository userRepository;

		[TestInitialize]
		public void Initialize()
		{
			userRepository = new UserRepository("DBConnection");
			userRepository.Clear().Wait();
		}

		[TestMethod]
		public void Create()
		{
			var user = new User
			{
				Name = "John",
				Surname = "Gold",
				Age = 18
			};

			var task = userRepository.Create(user);
			var result = task.Result;

			Assert.AreEqual(user.Name, result.Name);
			Assert.AreEqual(user.Surname, result.Surname);
			Assert.AreEqual(user.Age, result.Age);
		}

		[TestMethod]
		public void Read()
		{
			var user = new User
			{
				Name = "John",
				Surname = "Gold",
				Age = 18
			};
			int id = userRepository.Create(user).Result.Id;

			var result = userRepository.Read(u => u.Id == id).Result.First();

			Assert.AreEqual(user.Name, result.Name);
			Assert.AreEqual(user.Surname, result.Surname);
			Assert.AreEqual(user.Age, result.Age);
		}

		[TestMethod]
		public void Update()
		{
			var user = new User
			{
				Name = "John",
				Surname = "Gold",
				Age = 18
			};
			var addedUser = userRepository.Create(user).Result;

			addedUser.Age = 19;

			var updatedUser = userRepository.Update(addedUser).Result;

			Assert.AreEqual(addedUser.Id, updatedUser.Id);
			Assert.AreEqual(user.Name, updatedUser.Name);
			Assert.AreEqual(user.Surname, updatedUser.Surname);
			Assert.AreEqual(19, updatedUser.Age);
		}

		[TestMethod]
		public void Delete()
		{
			var user = new User
			{
				Name = "John",
				Surname = "Gold",
				Age = 18
			};
			var addedUser = userRepository.Create(user).Result;

			var deletedUser = userRepository.Delete(addedUser).Result;

			var result = userRepository.Read(u => u.Id == addedUser.Id).Result;

			Assert.AreEqual(addedUser.Id, deletedUser.Id);
			Assert.AreEqual(0, result.Count());
		}
	}
}
