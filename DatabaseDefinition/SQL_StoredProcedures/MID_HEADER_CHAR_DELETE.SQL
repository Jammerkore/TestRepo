--dv =============================================
--dv Create date: 3/23/2015
--dv Description: Deletes a header characteristic.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_DELETE]
	@HC_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM HEADER_CHAR_JOIN
	WHERE HC_RID = @HC_RID

	DELETE
	FROM HEADER_CHAR
	WHERE HC_RID = @HC_RID

	SELECT @@ROWCOUNT
END
GO


