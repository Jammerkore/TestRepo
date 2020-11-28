Dim ArgObj, DesktopPath, ProgramPath, AllUsersStartMenuPath
Set Shell = CreateObject("WScript.Shell")

'Connect to Windows Installer object
'insProductName = "Client"
'GetInstallPath = ""
'Set installer = CreateObject("WindowsInstaller.Installer")
'For i = 1 To installer.Products.Count
'    productcode = installer.Products.Item(i - 1)
'    productname = installer.ProductInfo(productcode, "InstalledProductName")
'    MsgBox("*" + productname + "*" + insProductName + "*")
'    If UCase(CStr(productname)) = UCase(insProductName) Then
'        MsgBox("*FoundIt* " + productcode)
'        GetInstallPath = installer.ProductInfo(productcode, "InstallLocation")
'        Exit For
'    End If
'Next
'Set installer = Nothing
'If GetInstallPath = "" then
'    MsgBox("Install path for " & insProductName & " not found")
'else
'    MsgBox("Install path for " & insProductName & " = " & GetInstallPath)
'End If

ProgramPath = Session.Property("CustomActionData") + "Client\"
'DesktopPath = Shell.SpecialFolders("Desktop")
AllUsersStartMenuPath = Shell.SpecialFolders("AllUsersStartMenu") + "\Programs\"
Set link = Shell.CreateShortcut(AllUsersStartMenuPath + "MIDRetail.lnk")
link.IconLocation = ProgramPath + "MIDRetail.exe,0"
link.TargetPath = ProgramPath + "MIDRetail.exe"
link.WindowStyle = 2
link.WorkingDirectory = ProgramPath
'link.Arguments = "1 2 3"
'link.Description = "MIDRetail"
'link.HotKey = "CTRL+ALT+SHIFT+X"
link.Save
