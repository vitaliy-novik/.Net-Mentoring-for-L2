set manager = CreateObject("ManagedPowrprof.PowerManager")

result = MsgBox ("Hibernate(Yes) or Sleep(No)?", vbYesNo, "Sleep/Hibernate")

Select Case result
Case vbYes
    manager.SetSuspendState(true)
Case vbNo
    manager.SetSuspendState(false)
End Select