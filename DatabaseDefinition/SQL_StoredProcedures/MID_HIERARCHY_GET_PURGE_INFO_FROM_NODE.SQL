--dv Create date:	9/4/2013
--dv Description:	Gets node purge data for the given node.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_GET_PURGE_INFO_FROM_NODE]
	@HN_RID int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT * FROM VW_HIERARCHY_GET_DATA                
	WHERE HN_RID=@HN_RID
		
END
GO
