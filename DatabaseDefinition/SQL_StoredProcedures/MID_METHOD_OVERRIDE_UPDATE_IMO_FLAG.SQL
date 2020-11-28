--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Sets the apply VSW flag for an override method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_UPDATE_IMO_FLAG]
	@METHOD_RID int, 
	@IMO_APPLY_VSW bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE METHOD_OVERRIDE 
	SET 
           IMO_APPLY_VSW = @IMO_APPLY_VSW
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO