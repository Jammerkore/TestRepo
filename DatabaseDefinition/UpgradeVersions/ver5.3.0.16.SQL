/*RELEASE 5.3.0.16 02-07-2014 - TT#950-MD -stodd - missing group allocation button*/
exec SP_MID_UPDATE_TEXT 301119,'Group Allocation',50,1,12,3,0	
exec [dbo].[SP_MID_UPDATE_TEXT] 301120,'Allocation By Store Report',50,1,12,3,0
GO