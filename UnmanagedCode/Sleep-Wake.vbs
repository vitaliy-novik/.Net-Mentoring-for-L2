set manager = CreateObject("ManagedPowrprof.PowerManager")

lastSleep = manager.GetLastSleepTime()
lastWake = manager.GetLastWakeTime()

result = MsgBox ("Last sleep time: " & lastSleep & vbNewLine & "Last wake time: "& lastWake)