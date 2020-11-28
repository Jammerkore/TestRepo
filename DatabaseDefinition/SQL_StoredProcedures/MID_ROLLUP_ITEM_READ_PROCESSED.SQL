--dv =============================================
--dv Create date: 3/6/2018
--dv Description: Reads processed rollup items
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_READ_PROCESSED]
	@PROCESS int,
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [HN_RID]
      ,[TIME_ID]
      ,[FV_RID]
      ,[ITEM_TYPE] 
	FROM ROLLUP_ITEM
	WHERE PROCESS = @PROCESS
		  AND PH_RID = @PH_RID
		  AND ITEM_PROCESSED IS NOT NULL

END
GO