--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Reads hierarchy type for the node rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_READ_TYPE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HN_TYPE 
	FROM HIERARCHY_NODE hn
    WHERE hn.HN_RID = @HN_RID

END
GO
