--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Updates the rule layer
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_COLOR_RULE_LAYER_UPDATE]
	@HDR_BC_RID INT,
	@LAYER_ID INT,
	@METHOD_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE BULK_COLOR_RULE_LAYER 
	SET 
		METHOD_RID = @METHOD_RID
	WHERE HDR_BC_RID = @HDR_BC_RID
		  AND LAYER_ID = @LAYER_ID
	
	SELECT @@ROWCOUNT;
END
GO