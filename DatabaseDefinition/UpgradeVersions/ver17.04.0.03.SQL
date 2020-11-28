/*RELEASE 17.04.0.03 8-19-2019 TT#xxxx - JSmith - Change one time purge flag to false */

update SYSTEM_OPTIONS
 set PERFORM_ONETIME_SPECIAL_PURGE_IND = '0'

GO
