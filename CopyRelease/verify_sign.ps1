.\LoadConfig files_sign.config

$buildpath = $appSettings["buildpath"]
$certpath = $buildpath + $appSettings["certpath"]

Get-ChildItem -Path $buildpath -Recurse -include MID*.exe |
  ForEach-Object { 
    sn -vf $_.FullName
    signtool verify /pa $_.FullName
    #corflags $_.FullName
  }

Get-ChildItem -Path $buildpath -Recurse -include MID*.dll |
  ForEach-Object { 
    sn -vf $_.FullName
    signtool verify /pa $_.FullName
    #corflags $_.FullName
  }
