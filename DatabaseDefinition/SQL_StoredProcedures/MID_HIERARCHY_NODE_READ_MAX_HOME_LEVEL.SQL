--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads the maximum home level for the product hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_NODE_READ_MAX_HOME_LEVEL] 
	@HOME_PH_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		CASE WHEN MAX_HOME_LEVEL IS NULL THEN 0 ELSE MAX_HOME_LEVEL END AS MAX_HOME_LEVEL
	FROM
	(
		SELECT MAX(HOME_LEVEL) AS MAX_HOME_LEVEL 
		FROM HIERARCHY_NODE 
		WHERE HOME_PH_RID = @HOME_PH_RID
	) AS T

END
GO