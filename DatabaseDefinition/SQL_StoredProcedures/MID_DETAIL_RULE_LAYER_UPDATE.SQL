--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Updates the rule layer
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DETAIL_RULE_LAYER_UPDATE]
	@HDR_RID INT,
	@LAYER_ID INT,
	@METHOD_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE DETAIL_RULE_LAYER 
	SET 
		METHOD_RID = @METHOD_RID
	WHERE HDR_RID = @HDR_RID
		  AND LAYER_ID = @LAYER_ID
	
	SELECT @@ROWCOUNT;
END
GO