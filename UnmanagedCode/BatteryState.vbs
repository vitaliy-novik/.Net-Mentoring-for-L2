set manager = CreateObject("ManagedPowrprof.PowerManager")

state = manager.GetSystemBatteryState()

result = MsgBox (state.Charging)