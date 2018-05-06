using System;
using System.Runtime.InteropServices;

namespace ManagedPowrprof
{
	[ComVisible(true)]
	[Guid("41EE1D7F-081C-4D18-9C7E-B7719EDB0EBF")]
	[ClassInterface(ClassInterfaceType.None)]
	public class PowerManager : IPowerManager
	{
		public string GetLastSleepTime()
		{
			IntPtr lastSleep = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(long)));
			int ntStatus = PowerManagementInterop.CallNtPowerInformation(
				15,
				IntPtr.Zero,
				0,
				lastSleep,
				(uint)Marshal.SizeOf(typeof(long))
				);
			long lastSleepTimeInSeconds = Marshal.ReadInt64(lastSleep, 0) / 10000000;
			TimeSpan time = TimeSpan.FromSeconds(lastSleepTimeInSeconds);
			return DateTime.Today.Add(time).ToString();
		}

		public string GetLastWakeTime()
		{
			IntPtr lastWake = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(long)));
			int ntStatus = PowerManagementInterop.CallNtPowerInformation(
				14,
				IntPtr.Zero,
				0,
				lastWake,
				(uint)Marshal.SizeOf(typeof(long))
				);
			long lastWakeTimeInSeconds = Marshal.ReadInt64(lastWake, 0) / 10000000;
			TimeSpan time = TimeSpan.FromSeconds(lastWakeTimeInSeconds);
			return DateTime.Today.Add(time).ToString();
		}

		public BattaryState GetSystemBatteryState()
		{
			IntPtr state = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(BattaryState)));
			int ntStatus = PowerManagementInterop.CallNtPowerInformation(
				5,
				IntPtr.Zero,
				0,
				state,
				(UInt32)Marshal.SizeOf(typeof(BattaryState))
				);

			return (BattaryState)Marshal.PtrToStructure(state, typeof(BattaryState));
		}

		public PowerInformaion GetSystemPowerInformation()
		{
			IntPtr state = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PowerInformaion)));
			int ntStatus = PowerManagementInterop.CallNtPowerInformation(
				12,
				IntPtr.Zero,
				0,
				state,
				(UInt32)Marshal.SizeOf(typeof(PowerInformaion))
				);

			return (PowerInformaion)Marshal.PtrToStructure(state, typeof(PowerInformaion));
		}

		public void SetSuspendState(bool hibernate)
		{
			PowerManagementInterop.SetSuspendState(hibernate, false, false);
		}
	}
}
