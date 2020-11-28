--dv ============================================= 
--dv Author:		John Smith 
--dv Create date: 11/27/2013 
--dv Modified:	11/27/2013 
--dv Description:	Returns datatable based on SQL Type
--dv ============================================= 
CREATE PROCEDURE [dbo].[SP_MID_GET_TABLE_FROM_TYPE] 
	@TABLE_TYPE int
AS
BEGIN 

DECLARE @CFWL MID_CHN_FOR_WK_LOCK_TYPE 
DECLARE @CFW MID_CHN_FOR_WK_TYPE 
DECLARE @CHW MID_CHN_HIS_WK_TYPE 
DECLARE @SFWL MID_ST_FOR_WK_LOCK_TYPE 
DECLARE @SFW MID_ST_FOR_WK_TYPE 
DECLARE @SHD MID_ST_HIS_DAY_TYPE 
DECLARE @SHW MID_ST_HIS_WK_TYPE 

if @TABLE_TYPE = 1 SELECT top 0 * FROM @CFWL 
else if @TABLE_TYPE = 2 SELECT top 0 * FROM @CFW 
else if @TABLE_TYPE = 3 SELECT top 0 * FROM @CHW 
else if @TABLE_TYPE = 4 SELECT top 0 * FROM @SFWL 
else if @TABLE_TYPE = 5 SELECT top 0 * FROM @SFW 
else if @TABLE_TYPE = 6 SELECT top 0 * FROM @SHD 
else if @TABLE_TYPE = 7 SELECT top 0 * FROM @SHW

END
GO