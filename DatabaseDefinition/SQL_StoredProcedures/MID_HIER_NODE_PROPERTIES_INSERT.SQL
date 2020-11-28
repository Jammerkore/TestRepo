--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Inserts a row into the hierarchy node properties table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_NODE_PROPERTIES_INSERT]
	@HN_RID int,
	@APPLY_HN_RID_FROM int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO HIER_NODE_PROPERTIES (HN_RID, APPLY_HN_RID_FROM) 
	VALUES (@HN_RID, @APPLY_HN_RID_FROM)

	SELECT @@ROWCOUNT;

END
GO
