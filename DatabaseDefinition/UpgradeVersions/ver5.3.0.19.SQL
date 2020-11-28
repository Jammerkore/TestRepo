/*RELEASE 5.3.0.19 02-25-2014 - TT#952-MD -stodd - change layout of Group Allocation Security*/
update SECURITY_FUNCTION_JOIN
set FUNC_TYPE = 0
where FUNC_ID = 880000 and FUNC_TYPE = 2 and FUNC_LEVEL = 0 and FUNC_PARENT = 0
go

update SECURITY_FUNCTION_JOIN
set FUNC_TYPE = 0
where FUNC_ID = 881000 and FUNC_TYPE = 2 and FUNC_LEVEL = 1 and FUNC_PARENT = 880000
go