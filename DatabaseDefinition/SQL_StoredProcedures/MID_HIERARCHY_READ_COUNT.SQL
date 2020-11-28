--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads the total count of the hierarchy nodes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_READ_COUNT]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM HIERARCHY_NODE

END
GO
