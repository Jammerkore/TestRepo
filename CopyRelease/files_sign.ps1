C:\scmvs2017\build\CopyRelease\LoadConfig C:\scmvs2017\build\CopyRelease\files_sign.config

function Get-AssemblyStrongName($assemblyPath)
{
	[System.Reflection.AssemblyName]::GetAssemblyName($assemblyPath).Name
}

$buildpath = $appSettings["buildpath"]
$certpath = $appSettings["certpath"]
$snkpath = "C:\Meh\" + $appSettings["snk"]
$batchpath = $appSettings["batchpath"]

Get-ChildItem -Path $buildpath -Recurse -include MID*.exe |
  ForEach-Object { 
    #sn -vf $_.FullName
    #corflags $_.FullName /32bit+ /Force
    #sn -R $_.FullName $snkpath
    $signdesc =  Get-AssemblyStrongName($_.FullName)
    signtool sign /f $certpath /p flow-BevpAQh /d $signdesc /t http://timestamp.verisign.com/scripts/timestamp.dll $_.FullName 
    #sn -vf $_.FullName
  }

Get-ChildItem -Path $buildpath -Recurse -include MID*.dll |
  ForEach-Object { 
    #sn -vf $_.FullName
    #corflags $_.FullName /32bit+ /Force
    #sn -R $_.FullName $snkpath
    $signdesc =  Get-AssemblyStrongName($_.FullName)
    signtool sign /f $certpath /p flow-BevpAQh /d $signdesc /t http://timestamp.verisign.com/scripts/timestamp.dll $_.FullName 
    #sn -vf $_.FullName
  }
  
  Get-ChildItem -Path $batchpath -Recurse -include *.exe |
  ForEach-Object { 
    #sn -vf $_.FullName
    #corflags $_.FullName /32bit+ /Force
    #sn -R $_.FullName $snkpath
    $signdesc =  Get-AssemblyStrongName($_.FullName)
    signtool sign /f $certpath /p flow-BevpAQh /d $signdesc /t http://timestamp.verisign.com/scripts/timestamp.dll $_.FullName 
    #sn -vf $_.FullName
  }

Get-ChildItem -Path $batchpath -Recurse -include log4net.dll |
  ForEach-Object { 
    #sn -vf $_.FullName
    #corflags $_.FullName /32bit+ /Force
    #sn -R $_.FullName $snkpath
    $signdesc =  Get-AssemblyStrongName($_.FullName)
    signtool sign /f $certpath /p flow-BevpAQh /d $signdesc /t http://timestamp.verisign.com/scripts/timestamp.dll $_.FullName 
    #sn -vf $_.FullName
  }

Get-ChildItem -Path $buildpath -Recurse -include Infragistics2*.dll |
  ForEach-Object { 
    #sn -vf $_.FullName
    #corflags $_.FullName /32bit+ /Force
    #sn -R $_.FullName $snkpath
    $signdesc =  Get-AssemblyStrongName($_.FullName)
    signtool sign /f $certpath /p flow-BevpAQh /d $signdesc /t http://timestamp.verisign.com/scripts/timestamp.dll $_.FullName 
    #sn -vf $_.FullName
  }