using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagedPowrprof;

namespace ManagedPowerprofTests
{
	[TestClass]
	public class PowerManagementInteropTests
	{
		private PowerManager powerManager;

		[TestInitialize]
		public void Initialize()
		{
			powerManager = new PowerManager();
		}

		[TestMethod]
		public void LastSleepTime()
		{
			DateTime dateTime = powerManager.GetLastSleepTime();
			Console.WriteLine(dateTime.ToString());
		}

		[TestMethod]
		public void LastWakeTime()
		{
			DateTime dateTime = powerManager.GetLastWakeTime();
			Console.WriteLine(dateTime.ToString());
		}

		[TestMethod]
		public void SystemBatteryState()
		{
			BattaryState btState = powerManager.GetSystemBatteryState();
		}

		[TestMethod]
		public void SystemPowerInformation()
		{
			PowerInformaion pState = powerManager.GetSystemPowerInformation();
		}

		[TestMethod]
		public void SetSuspendState()
		{
			powerManager.SetSuspendState(false);
		}
	}
}
