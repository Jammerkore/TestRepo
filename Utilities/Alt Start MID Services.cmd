::==============================================================
:: Starts the MID Allocation Services one-at-a-time. 
:: Each service waits until the previous service has started.
::==============================================================

rem start all services
:: maxloop = number of retries when starting a service fails
set /a maxloop = 4 
:: waittime = number of seconds to wait before checking if service is up (in seconds)
set /a waittime = 10

::====================================
:: Control Service
::====================================
rem net start MIDRetailControlService
set /a loopcnt = 0
net start MIDRetailControlService
GOTO :checkcontrol

:waitcheckcontrol
rem waiting... 
ping -n %waittime% 127.0.0.1 > NUL 2>&1
rem if Control stopped, start again
SC query MIDRetailControlService| FIND "STOPPED" 
IF %errorlevel% ==0 (
	IF %loopcnt% lss %maxloop% ( 
		set /a loopcnt+=1
		net start MIDRetailControlService
	) ELSE GOTO :stop
) 

:checkcontrol
rem check if Control is running
SC query MIDRetailControlService| FIND "RUNNING" 
IF %errorlevel% == 1 GOTO :waitcheckcontrol


::====================================
:: Hierarchy Service
::====================================
rem net start MIDRetailHierarchyService
set /a loopcnt = 0
net start MIDRetailHierarchyService
GOTO :checkHierarchy

:waitcheckhierarchy
rem waiting... 
ping -n %waittime% 127.0.0.1 > NUL 2>&1
rem if Hierarchy stopped, start again
SC query MIDRetailHierarchyService| FIND "STOPPED" 
IF %errorlevel% ==0 (
	IF %loopcnt% lss %maxloop% ( 
		set /a loopcnt+=1
		net start MIDRetailHierarchyService
	) ELSE GOTO :stop
) 

:checkHierarchy
rem check if Hierarchy is running
SC query MIDRetailHierarchyService| FIND "RUNNING" 
IF %errorlevel% == 1 GOTO :waitcheckhierarchy


::====================================
:: Scheduler Service
::====================================
rem net start MIDRetailSchedulerService
set /a loopcnt = 0
net start MIDRetailSchedulerService
GOTO :checkScheduler

:waitcheckscheduler
rem waiting... 
ping -n %waittime% 127.0.0.1 > NUL 2>&1
rem if Scheduler stopped, start again
SC query MIDRetailSchedulerService| FIND "STOPPED" 
IF %errorlevel% ==0 (
	IF %loopcnt% lss %maxloop% ( 
		set /a loopcnt+=1
		net start MIDRetailSchedulerService
	) ELSE GOTO :stop
) 

:checkScheduler
rem check if Scheduler is running
SC query MIDRetailSchedulerService| FIND "RUNNING" 
IF %errorlevel% == 1 GOTO :waitcheckscheduler


::====================================
:: Store Service
::====================================
rem net start MIDRetailStoreService
set /a loopcnt = 0
net start MIDRetailStoreService
GOTO :checkStore

:waitcheckstore
rem waiting... 
ping -n %waittime% 127.0.0.1 > NUL 2>&1
rem if Store stopped, start again
SC query MIDRetailStoreService| FIND "STOPPED" 
IF %errorlevel% ==0 (
	IF %loopcnt% lss %maxloop% ( 
		set /a loopcnt+=1
		net start MIDRetailStoreService
	) ELSE GOTO :stop
) 

:checkStore
rem check if Store is running
SC query MIDRetailStoreService| FIND "RUNNING" 
IF %errorlevel%  == 1 GOTO :waitcheckstore

:stop


