--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads the maximum home level for the rollup item
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_READ_MAX_HOME_LEVEL] 
	@PROCESS INT,
	@PH_RID INT,
	@ITEM_TYPE INT,
	@FV_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		CASE WHEN MAX_HOME_LEVEL IS NULL THEN 0 ELSE MAX_HOME_LEVEL END AS MAX_HOME_LEVEL
	FROM
	(
		SELECT 
			MAX(HOME_LEVEL) AS MAX_HOME_LEVEL 
		FROM ROLLUP_ITEM
		WHERE PROCESS = @PROCESS
					 AND PH_RID = @PH_RID
					 AND ITEM_TYPE = @ITEM_TYPE
					 AND FV_RID = @FV_RID
					 AND ITEM_PROCESSED IS NULL
	) AS T

END
GO
