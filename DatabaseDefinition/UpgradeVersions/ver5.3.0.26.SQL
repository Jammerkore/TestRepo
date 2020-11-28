/*RELEASE 5.3.0.26 03-18-2014 - TT#1078-MD -stodd - Assortment showing when assortment not installed */
update SECURITY_FUNCTION_JOIN
set FUNC_TYPE = 2
where FUNC_ID = 867000 and FUNC_TYPE = 0 and FUNC_LEVEL = 1 and FUNC_PARENT = 860000
go