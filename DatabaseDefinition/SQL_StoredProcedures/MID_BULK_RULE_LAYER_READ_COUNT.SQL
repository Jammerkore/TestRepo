--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Reads a count for the rule layer for the header and layer
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_RULE_LAYER_READ_COUNT]
	@HDR_RID INT,
	@LAYER_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) MyCount 
	FROM BULK_RULE_LAYER 
	WHERE HDR_RID = @HDR_RID
		  AND LAYER_ID = @LAYER_ID

END
GO