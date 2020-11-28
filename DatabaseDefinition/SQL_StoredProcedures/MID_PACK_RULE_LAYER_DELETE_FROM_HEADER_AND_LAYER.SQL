--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Deletes the rule layer for the header and layer
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PACK_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER]
	@HDR_PACK_RID INT,
	@LAYER_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PACK_RULE_LAYER
    WHERE HDR_PACK_RID = @HDR_PACK_RID
          AND LAYER_ID = @LAYER_ID

	SELECT @@ROWCOUNT;
END
GO


