--dv =============================================
--dv Create date: 12/18/2013
--dv Description: Delete store characteristics.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_DELETE]
	@SC_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM STORE_CHAR_JOIN
	WHERE SC_RID = @SC_RID

	DELETE
	FROM STORE_CHAR
	WHERE SC_RID = @SC_RID

	SELECT @@ROWCOUNT
END
GO


