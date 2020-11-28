--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Deletes a size alternate model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_ALTERNATE_MODEL_DELETE]
	@SIZE_ALTERNATE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_ALTERNATE_MODEL 
	WHERE SIZE_ALTERNATE_RID = @SIZE_ALTERNATE_RID

	SELECT @@ROWCOUNT;
END
GO
