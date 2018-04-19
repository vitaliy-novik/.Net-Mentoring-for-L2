using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample03.E3SClient.Entities;
using Sample03.E3SClient;
using System.Configuration;
using System.Linq;

namespace Sample03
{
	[TestClass]
	public class E3SProviderTests
	{
		E3SEntitySet<EmployeeEntity> employees;

		[TestInitialize]
		public void Initialize()
		{
			employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
		}

		[TestMethod]
		public void WithoutProvider()
		{
			var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
			var res = client.SearchFTS<EmployeeEntity>("workstation:(EPRUIZHW0249)", 0, 1);

			foreach (var emp in res)
			{
				Console.WriteLine("{0} {1}", emp.nativename, emp.workstation);
			}
		}

		[TestMethod]
		public void WithoutProviderNonGeneric()
		{
			var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
			var res = client.SearchFTS(typeof(EmployeeEntity), "workstation:(EPRUIZHW0249)", 0, 10);

			foreach (var emp in res.OfType<EmployeeEntity>())
			{
				Console.WriteLine("{0} {1}", emp.nativename, emp.workstation);
			}
		}


		[TestMethod]
		public void WithProvider_Equals_LeftMember_RightConst()
		{
			foreach (var emp in employees.Where(e => e.workstation == "EPRUIZHW0249"))
			{
				Console.WriteLine("{0} {1}", emp.nativename, emp.workstation);
			}
		}

		[TestMethod]
		public void WithProvider_Equals_LeftConst_RightMember()
		{
			foreach (var emp in employees.Where(e => "EPRUIZHW0249" == e.workstation))
			{
				Console.WriteLine("{0} {1}", emp.nativename, emp.workstation);
			}
		}

		[TestMethod]
		public void WithProvider_StartsWith()
		{
			foreach (var emp in employees.Where(e => e.workstation.StartsWith("EPRUIZHW024")))
			{
				Console.WriteLine("{0} {1}", emp.nativename, emp.workstation);
			}
		}

		[TestMethod]
		public void WithProvider_EndsWith()
		{
			foreach (var emp in employees.Where(e => e.workstation.EndsWith("PRUIZHW0249")))
			{
				Console.WriteLine("{0} {1}", emp.nativename, emp.workstation);
			}
		}

		[TestMethod]
		public void WithProvider_Contains()
		{
			var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

			foreach (var emp in employees.Where(e => e.workstation.Contains("PRUIZHW024")))
			{
				Console.WriteLine("{0} {1}", emp.nativename, emp.workstation);
			}
		}

		[TestMethod]
		public void WithProvider_And()
		{
			var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

			foreach (var emp in employees.Where(e => 
				e.firstname == "Mihail" && e.room.Contains("4 Floor")))
			{
				Console.WriteLine("{0} {1}", emp.nativename, emp.room);
			}
		}
	}
}
