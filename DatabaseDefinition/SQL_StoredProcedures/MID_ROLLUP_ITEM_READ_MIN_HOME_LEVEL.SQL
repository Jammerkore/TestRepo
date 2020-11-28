--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads the minimum home level for the rollup item
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_READ_MIN_HOME_LEVEL] 
	@PROCESS INT,
	@PH_RID INT,
	@ITEM_TYPE INT,
	@FV_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		CASE WHEN MIN_HOME_LEVEL IS NULL THEN 0 ELSE MIN_HOME_LEVEL END AS MIN_HOME_LEVEL
	FROM
	(
		SELECT 
			MIN(HOME_LEVEL) AS MIN_HOME_LEVEL 
		FROM ROLLUP_ITEM
		WHERE PROCESS = @PROCESS
					 AND PH_RID = @PH_RID
					 AND ITEM_TYPE = @ITEM_TYPE
					 AND FV_RID = @FV_RID
					 AND ITEM_PROCESSED IS NULL
	) AS T

END

GO
