/*RELEASE 5.3.0.31 04-17-2014 - TT#1126-MD -RMatelic -  Show Details needs consistent “controls” as the main Workspace Explorer */
IF NOT EXISTS(SELECT * FROM APPLICATION_TEXT where TEXT_CODE = 900891)
BEGIN
exec SP_MID_SET_TEXT 900891,'Grid',30,1,2,0,0
END
go