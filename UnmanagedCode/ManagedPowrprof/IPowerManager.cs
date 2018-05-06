using System;
using System.Runtime.InteropServices;

namespace ManagedPowrprof
{
	[ComVisible(true)]
	[Guid("4F3C961C-38F3-46BD-B463-F827C030BE93")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IPowerManager
	{
		void SetSuspendState(bool hibernate);

		string GetLastSleepTime();

		string GetLastWakeTime();

		BattaryState GetSystemBatteryState();

		PowerInformaion GetSystemPowerInformation();
	}
}
