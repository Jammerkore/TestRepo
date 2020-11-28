--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Reads hierarchy rids based on the home product hierarchy rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_READ_FROM_HOME_PH]
	@HOME_PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HN_RID
	FROM HIERARCHY_NODE 
	WHERE HOME_PH_RID=@HOME_PH_RID
	ORDER BY HOME_LEVEL DESC

END
GO
