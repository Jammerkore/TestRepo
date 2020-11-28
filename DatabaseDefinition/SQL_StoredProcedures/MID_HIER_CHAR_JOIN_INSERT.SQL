--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row on the hierarchy characteristic join table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_JOIN_INSERT]
	@HN_RID int,
	@HC_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO HIER_CHAR_JOIN(HN_RID, HC_RID) 
	VALUES (@HN_RID, @HC_RID)

	SELECT @@ROWCOUNT;

END
GO
