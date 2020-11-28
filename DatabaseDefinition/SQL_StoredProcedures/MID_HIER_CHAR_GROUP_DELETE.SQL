--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row on the hierarchy characteristic group table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_GROUP_DELETE]
	@HCG_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM HIER_CHAR_GROUP WITH (ROWLOCK) 
	WHERE HCG_RID=@HCG_RID

	SELECT @@ROWCOUNT;

END
GO
