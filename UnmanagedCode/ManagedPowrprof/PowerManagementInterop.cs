using System;
using System.Runtime.InteropServices;

namespace ManagedPowrprof
{
	public class PowerManagementInterop
	{
		[DllImport("Powrprof.dll", SetLastError = true)]
		internal static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

		[DllImport("Powrprof.dll", SetLastError = true)]
		internal static extern int CallNtPowerInformation(
			int informationLevel,
			[In] IntPtr lpInputBuffer,
			uint nInputBufferSize,
			[In, Out] IntPtr lpOutputBuffer,
			uint nOutputBufferSize
		);

		[DllImport("Powrprof.dll", SetLastError = true)]
		internal static extern int CallNtPowerInformation(
			int informationLevel,
			[In] bool lpInputBuffer,
			uint nInputBufferSize,
			[In, Out] IntPtr lpOutputBuffer,
			uint nOutputBufferSize
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BattaryState
	{
		public bool AcOnLine;
		public bool BatteryPresent;
		public bool Charging;
		public bool Discharging;
		public byte Spare0;
		public byte Spare1;
		public byte Spare2;
		public byte Spare3;
		public uint MaxCapacity;
		public uint RemainingCapacity;
		public int Rate;
		public uint EstimatedTime;
		public uint DefaultAlert1;
		public uint DefaultAlert2;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PowerInformaion
	{
		public uint MaxIdlenessAllowed;
		public uint Idleness;
		public uint TimeRemaining;
		public byte CoolingMode;
	}
}
