--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Deletes size alternate alternate sizes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_ALT_ALTERNATE_SIZE_DELETE]
	@SIZE_ALTERNATE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_ALT_ALTERNATE_SIZE 
	WHERE SIZE_ALTERNATE_RID = @SIZE_ALTERNATE_RID

	SELECT @@ROWCOUNT;
END
GO


