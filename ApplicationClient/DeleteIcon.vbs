Dim AllUsersStartMenuPath, FileToDelete
Set Shell = CreateObject("WScript.Shell")
Set FSO = CreateObject("Scripting.FileSystemObject")

AllUsersStartMenuPath = Shell.SpecialFolders("AllUsersStartMenu") + "\Programs\"
IF FSO.FileExists(AllUsersStartMenuPath + "MIDRetail.lnk") THEN
   FSO.DeleteFile (AllUsersStartMenuPath + "MIDRetail.lnk")
END IF
