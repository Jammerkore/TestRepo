--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Updates the custom override low level model rid on a method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_UPDATE_CUSTOM_OLL]
	@METHOD_RID int,
	@CUSTOM_OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE METHOD 
	SET
		CUSTOM_OLL_RID = @CUSTOM_OLL_RID
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO