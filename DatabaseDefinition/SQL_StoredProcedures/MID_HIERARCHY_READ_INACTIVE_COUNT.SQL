--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads the inactive count of the hierarchy nodes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_READ_INACTIVE_COUNT]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM HIERARCHY_NODE 
	WHERE ACTIVE_IND = '0'

END
GO
