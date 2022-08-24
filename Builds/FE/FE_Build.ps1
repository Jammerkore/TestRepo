$params=$args[0]
#Set date
$dateToday = "{0:yyyy-MM-dd}" -f (get-date)

#Start Transcripts
Start-Transcript -Path c:\DevOps\logs\BE_BUILD-$dateToday.log

Write-Host "---------------------------------------"
write-host "Starting FE build..."
Write-Host "---------------------------------------"
$SCMFolder = "C:\SCMVS2017"
$workFolder = "C:\Work"

Copy-Item -path "C:\Build Information\SCM Naming Conventions.txt" -Destination "c:\devops\backup\SCM Naming Conventions-$dateToday.txt"
Copy-Item -path "C:\Build Information\2203 Builds\Batch Scripts\WEBUI\STEP_2_Replace_2203_DEV_Version_Numbers.bat" -Destination "c:\devops\backup\STEP_2_Replace_2203_DEV_Version_Numbers-$dateToday.bat"


if (Test-Path -Path $SCMFolder) {
	
    Write-Host "Deleteing $SCMFolder folder.  Please wait..."
	Remove-Item $SCMFolder -Recurse -Force
	
} else {
	
    "$SCMFolder doesn't exist. Skipping..."
	
}

Write-Host "---------------------------------------"

if (Test-Path -Path $workFolder) {
	
    Write-Host "Deleteing $workFolder folder.  Please wait..."
	Remove-Item $workFolder -Recurse -Force
	
} else {
	
    "$workFolder doesn't exist. Skipping..."
	
}

Write-Host "---------------------------------------"
Write-Host "Getting release numbers:"
Write-Host "---------------------------------------"
$currentRelease = (gci "\\midretail14\MIDRetail\QA Build\Base 22.03" | ? { $_.PSIsContainer } | sort CreationTime)[-1]
$currentRelease = $currentRelease -replace 'Logility-RO '
$currentRelease = $currentRelease -replace ".{6}$"
Write-Host "Current Release: $currentRelease"
Write-Host "---------------------------------------"
$FILE = Get-Content "C:\Build Information\SCM Naming Conventions.txt"
$results=@()
foreach ($LINE in $FILE) 
{
$out=Write-Output "$LINE"
$results+=$out
}
$namingCurrent = $results[4]
$namingDev = $results[8]

Write-Host "SCMNaming Conventions File Values"
Write-Host "---------------------------------------"
Write-Host "Current: $namingCurrent will be updated to $currentRelease"
Write-Host "Previous Dev $namingDev will be updated to $namingCurrent"

try {
	
	$file = 'C:\Build Information\SCM Naming Conventions.txt'
	$content = Get-Content -Path $file
	$content[4] = $currentRelease
	$content[8] = $namingCurrent
	$content | Set-Content -Path $file

} catch {
	
	Write-Host "Error: $($PSItem.ToString())"
	throw $_
	
}

Write-Host "---------------------------------------"
Write-Host ""


try {
	
	$file = 'C:\Build Information\2203 Builds\Batch Scripts\WEBUI\STEP_2_Replace_2203_DEV_Version_Numbers.bat'
	Write-Host "set PreviousVersionNumber=`"$namingCurrent`""
	Write-Host "set CurrentVersionNumber=`"$currentRelease`""
	$content = Get-Content -Path $file
	$content[3] = "set PreviousVersionNumber=`"$namingCurrent`""
	$content[4] = "set CurrentVersionNumber=`"$currentRelease`""
	$content | Set-Content -Path $file

} catch {
	
	Write-Host "Error: $($PSItem.ToString())"
	throw $_
	
}

Write-Host "---------------------------------------"
Write-Host "Pull GIT Code"

try {
	
	Set-Location -Path c:\buildfolder
	
	git clone -b main https://ASI-ALL@dev.azure.com/ASI-ALL/RD-PRODUCT/_git/RO-Foundation "C:\SCMVS2017\RO Voyager Foundation\Foundation"
	git clone -b main https://ASI-ALL@dev.azure.com/ASI-ALL/RD-PRODUCT/_git/RO-MODA "C:\SCMVS2017\Voyager RO MoDA 2203"
	git clone -b main https://ASI-ALL@dev.azure.com/ASI-ALL/RD-PRODUCT/_git/RO-FrontEnd "C:\SCMVS2017\Voyager RO UI Dev 2203\Integrated"
	git clone -b main https://ASI-ALL@dev.azure.com/ASI-ALL/RD-PRODUCT/_git/RO-FrontEnd "C:\SCMVS2017\Voyager RO UI TEST 2203\TEST1 Integrated"
	
} catch {
	
	Write-Host "Error: $($PSItem.ToString())"
	throw $_

}

try { 

	$path = "C:\SCMVS2017\Voyager Foundation\DocuShow"
	If(!(test-path $path)) 
	{
		Write-Host "Creating C:\SCMVS2017\Voyager Foundation\DocuShow"
		New-Item -ItemType Directory -Force -Path $path
		Write-Host "Copy files to C:\SCMVS2017\Voyager Foundation\DocuShow"
		Copy-Item -Path "C:\SCMVS2017\RO Voyager Foundation\Foundation\src\Logility.Foundation.Web.Docushow\*" -Destination "C:\SCMVS2017\Voyager Foundation\DocuShow\" -Recurse
	
	} else {
		
		Write-Host "C:\SCMVS2017\Voyager Foundation\DocuShow already exits.  This means there is an issue."
		Break
	
	}
	
} catch {
	
	Write-Host "Error: $($PSItem.ToString())"
	throw $_
	
}

Set-Location -Path "C:\Build Information\2203 Builds\Batch Scripts\WEBUI"

Try {
	
	Write-Host "Replacing Version Numbers.  Please wait..."
	
	start-process "C:\Build Information\2203 Builds\Batch Scripts\WEBUI\STEP_2_Replace_2203_DEV_Version_Numbers.bat" -wait

	Write-Host "Replacing Version Numbers complete."
	
} Catch {
	
	Write-Host "Error: $($PSItem.ToString())"
	throw $_	
	
}

Try {
	
	Write-Host "Copying Dev Configurations.  Please wait..."
	
	start-process "C:\Build Information\2203 Builds\Batch Scripts\WEBUI\STEP_3_Copy_DEV_Configurations.bat" -wait

	Write-Host "Copy complete."
	
} Catch {
	
	Write-Host "Error: $($PSItem.ToString())"
	throw $_	
	
}


Try {
	
	Write-Host "Build Dev.  Please wait..."
	
	start-process "C:\Build Information\2203 Builds\Batch Scripts\WEBUI\STEP_4_Build_DEV.bat" -wait

	Write-Host "Build complete."
	
} Catch {
	
	Write-Host "Error: $($PSItem.ToString())"
	throw $_	
	
}


Try {
	
	Write-Host "Compressing Dev.  Please wait..."
	
	$VersionNumber = $currentRelease

	Write-Output 'Creating Directory in DEV'
	New-Item -Path "C:\Daily Release\DEV\Version 2203\ROWEB $VersionNumber" -ItemType Directory

	Write-Output 'Creating ROWEB DEV.zip'
	Compress-Archive -Path "C:\SCMVS2017\Voyager RO UI DEV 2203\Integrated\deploy\Release\install\*" -CompressionLevel Fastest -DestinationPath "C:\Daily Release\DEV\Version 2203\ROWEB $VersionNumber\ROWEB DEV.zip"

	Write-Host "Compress complete."
	
} Catch {
	
	Write-Host "Error: $($PSItem.ToString())"
	throw $_	
	
}

Write-Host "Build Dev Complete."

