rmdir /s /q "C:\TEMP\Installer"
mkdir "C:\TEMP\Installer"

mkdir "C:\TEMP\Installer\Batch"
mkdir "C:\TEMP\Installer\Batch\StoreDelete"
mkdir "C:\TEMP\Installer\Batch\Transactions"
mkdir "C:\TEMP\Installer\Batch\HierarchyLevelMaintenance"

xcopy "C:\MIDInstaller\MIDInstallerBase\MID Advanced Allocation Batch" "C:\TEMP\Installer\Batch"  /C /Q /Y 

xcopy "C:\scmvs2013\Build\BatchOnlyMode\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y  

xcopy "C:\scmvs2013\Build\SchedulerJobManager\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y  

xcopy "C:\scmvs2013\Build\AllocationScheduler\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\AllocationScheduler\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\AllocationScheduler\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\BatchComp\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\BatchComp\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\BatchComp\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\BuildPackCriteriaLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\BuildPackCriteriaLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\BuildPackCriteriaLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\ChainSetPercentages\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ChainSetPercentages\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ChainSetPercentages\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\ComputationDriver\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ComputationDriver\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ComputationDriver\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\ConvertFilters\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ConvertFilters\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ConvertFilters\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\ColorCodesLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\ColorCodesLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ColorCodesLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\DailyPercentages\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y
xcopy "C:\scmvs2013\Build\DailyPercentages\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\DailyPercentages\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\DetermineHierarchyActivity\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\DetermineHierarchyActivity\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\DetermineHierarchyActivity\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\HeaderAllocationLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\HeaderAllocationLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\HeaderAllocationLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\HeaderLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\HeaderLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\HeaderLoad\*.txt" "C:\TEMP\Installer\Batch"  /C /Q /Y

xcopy "C:\scmvs2013\Build\HeaderReconcile\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\HeaderReconcile\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\HeaderReconcile\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\HierarchyLevelMaintenance\bin\Release" "C:\TEMP\Installer\Batch\HierarchyLevelMaintenance"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\HierarchyLevelMaintenance\*.xml" "C:\TEMP\Installer\Batch\HierarchyLevelMaintenance"  /C /Q /Y
xcopy "C:\scmvs2013\Build\HierarchyLevelMaintenance\*.txt" "C:\TEMP\Installer\Batch\HierarchyLevelMaintenance"  /C /Q /Y

xcopy "C:\scmvs2013\Build\HierarchyLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\HierarchyLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\HierarchyLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\HierarchyReclass\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y
xcopy "C:\scmvs2013\Build\HierarchyReclass\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\HierarchyReclass\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\HistoryPlanLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y
xcopy "C:\scmvs2013\Build\HistoryPlanLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\HistoryPlanLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\PlanForecasting\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\PlanForecasting\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\PlanForecasting\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\Purge\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\Purge\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Purge\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\PushToBackStock\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\PushToBackStock\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\PushToBackStock\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\ReBuildIntransit\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\ReBuildIntransit\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ReBuildIntransit\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\RelieveHeaders\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\RelieveHeaders\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\RelieveHeaders\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\RelieveIntransit\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\RelieveIntransit\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\RelieveIntransit\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\ROLLUP\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\ROLLUP\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ROLLUP\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\ScheduleInterface\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\ScheduleInterface\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ScheduleInterface\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\SizeCodesLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\SizeCodesLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\SizeCodesLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\SizeConstraintsLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\SizeConstraintsLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\SizeConstraintsLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\SizeCurveGenerate\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\SizeCurveGenerate\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\SizeCurveGenerate\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\SizeCurveLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\SizeCurveLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\SizeCurveLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\SizeDayToWeekSummary\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\SizeDayToWeekSummary\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\SizeDayToWeekSummary\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\SpecialRequest\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\SpecialRequest\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\SpecialRequest\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\StoreLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\StoreLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\StoreLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

xcopy "C:\scmvs2013\Build\StoreDeleteWin\bin\Release" "C:\TEMP\Installer\Batch\StoreDelete"  /C /Q /Y 

xcopy "C:\scmvs2013\Build\StoreEligibility\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\StoreEligibility\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\StoreEligibility\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y


xcopy "C:\scmvs2013\Build\VSWLoad\bin\Release" "C:\TEMP\Installer\Batch"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\VSWLoad\*.xml" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y
xcopy "C:\scmvs2013\Build\VSWLoad\*.txt" "C:\TEMP\Installer\Batch\Transactions"  /C /Q /Y

mkdir "C:\TEMP\Installer\Client"
mkdir "C:\TEMP\Installer\Client\HelpFiles"
mkdir "C:\TEMP\Installer\Client\HelpFiles\Images"
mkdir "C:\TEMP\Installer\Graphics"

xcopy "C:\MIDInstaller\MIDInstallerBase\MID Advanced Allocation for SQL Server\Graphics" "C:\TEMP\Installer\Graphics"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Graphics\*.ico" "C:\TEMP\Installer\Graphics"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Graphics\*.bmp" "C:\TEMP\Installer\Graphics"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Graphics\*.gif" "C:\TEMP\Installer\Graphics"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Graphics\*.jpg" "C:\TEMP\Installer\Graphics"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Graphics\*.png" "C:\TEMP\Installer\Graphics"  /C /Q /Y

xcopy "C:\MIDInstaller\MIDInstallerBase\MID Advanced Allocation for SQL Server\MID Advanced Allocation" "C:\TEMP\Installer\Client"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\ApplicationClient\bin\Release\*.exe" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ApplicationClient\bin\Release\*.dll" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\cmvs2013\Build\ApplicationClient\HelpFiles\*.html" "C:\TEMP\Installer\Client\HelpFiles"  /C /Q /Y
xcopy "C:\cmvs2013\Build\ApplicationClient\HelpFiles\Images\*.png" "C:\TEMP\Installer\Client\HelpFiles\Images"  /C /Q /Y
xcopy "C:\scmvs2013\build\Windows\bin\Release\*.dll" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\build\CONTROL\bin\Release\*.dll" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\build\Data\bin\Release\*.dll" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\Build\ApplicationClient\bin\Release\*.config" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Windows\bin\Release\*.dll" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Graphics\MIDAllocation.ico" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Graphics\MIDRetail.ico" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\Build\AutoUpgrade\bin\Release\*.exe" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\Build\AutoUpgrade\bin\Release\*.dll" "C:\TEMP\Installer\Client"  /C /Q /Y
xcopy "C:\scmvs2013\Build\CONTROL\StyleFiles\*.isl" "C:\TEMP\Installer\Client"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\DisplayMessage\bin\Release\*.exe" "C:\TEMP\Installer\Client"  /C /Q /Y 
xcopy "\\Midretail14\tools\Crystal Reports VS2013 - V13_0_12\CRRuntime_*.msi" "C:\TEMP\Installer\Client"  /C /Q /Y 

mkdir "C:\TEMP\Installer\Control Service"
xcopy "C:\MIDInstaller\MIDInstallerBase\MID Advanced Allocation Control Service" "C:\TEMP\Installer\Control Service"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\Service\Control\bin\Release" "C:\TEMP\Installer\Control Service"  /C /Q /Y 


mkdir "C:\TEMP\Installer\Hierarchy Service"
xcopy "C:\MIDInstaller\MIDInstallerBase\MID Advanced Allocation Hierarchy Service" "C:\TEMP\Installer\Hierarchy Service"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\Service\Hierarchy\bin\Release" "C:\TEMP\Installer\Hierarchy Service"  /C /Q /Y 


mkdir "C:\TEMP\Installer\Scheduler Service" 
xcopy "C:\MIDInstaller\MIDInstallerBase\MID Advanced Allocation Scheduler Service" "C:\TEMP\Installer\Scheduler Service"  /C /Q /Y
xcopy "C:\scmvs2013\Build\Service\Scheduler\bin\Release" "C:\TEMP\Installer\Scheduler Service"  /C /Q /Y 


mkdir "C:\TEMP\Installer\Store Service"
xcopy "C:\MIDInstaller\MIDInstallerBase\MID Advanced Allocation Store Service" "C:\TEMP\Installer\Store Service"  /C /Q /Y 
xcopy "C:\scmvs2013\Build\Service\Store\bin\Release" "C:\TEMP\Installer\Store Service"  /C /Q /Y 

rem "C:\Program Files\WinZip\WZZIP.EXE" -r -p "C:\TEMP\Installer\Batch.zip" "C:\TEMP\Installer\Batch\*.*" 
rem "C:\Program Files\WinZip\WZZIP.EXE" -r -p "C:\TEMP\Installer\Client.zip" "C:\TEMP\Installer\Client\*.*"
rem "C:\Program Files\WinZip\WZZIP.EXE" -r -p "C:\TEMP\Installer\Graphics.zip" "C:\TEMP\Installer\Graphics\*.*"
rem "C:\Program Files\WinZip\WZZIP.EXE" -r -p "C:\TEMP\Installer\ControlService.zip" "C:\TEMP\Installer\ControlService\*.*"
rem "C:\Program Files\WinZip\WZZIP.EXE" -r -p "C:\TEMP\Installer\HierarchyService.zip" "C:\TEMP\Installer\HierarchyService\*.*"
rem "C:\Program Files\WinZip\WZZIP.EXE" -r -p "C:\TEMP\Installer\SchedulerService.zip" "C:\TEMP\Installer\SchedulerService\*.*"
rem "C:\Program Files\WinZip\WZZIP.EXE" -r -p "C:\TEMP\Installer\StoreService.zip" "C:\TEMP\Installer\StoreService\*.*"

del "C:\TEMP\Installer\Batch\*.pdb"
del "C:\TEMP\Installer\Batch\StoreDelete\*.pdb"
del "C:\TEMP\Installer\Client\*.pdb"
del "C:\TEMP\Installer\Control Service\*.pdb"
del "C:\TEMP\Installer\Hierarchy Service\*.pdb"
del "C:\TEMP\Installer\Scheduler Service\*.pdb"
del "C:\TEMP\Installer\Store Service\*.pdb"

del "C:\TEMP\Installer\Batch\*.vshost.*"
del "C:\TEMP\Installer\Batch\StoreDelete\*.vshost.*"
del "C:\TEMP\Installer\Client\*.vshost.*"
del "C:\TEMP\Installer\Control Service\*.vshost.*"
del "C:\TEMP\Installer\Hierarchy Service\*.vshost.*"
del "C:\TEMP\Installer\Scheduler Service\*.vshost.*"
del "C:\TEMP\Installer\Store Service\*.vshost.*"
