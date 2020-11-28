--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Deletes top X rows from the store forecast week table
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_FORECAST_WEEK6_DELETE_FOR_PURGE_DATE] 
	@COMMIT_LIMIT int,
	@TIME_ID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE TOP (@COMMIT_LIMIT) sf WITH (TABLOCK) 
    FROM STORE_FORECAST_WEEK6 sf WITH (INDEX(STORE_FORECAST_WEEK6_PK))
    WHERE sf.TIME_ID < @TIME_ID

	SELECT @@ROWCOUNT;

END
GO