--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Deletes the rule layer for the header and layer
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_COLOR_RULE_LAYER_DELETE_FROM_HEADER_AND_LAYER]
	@HDR_BC_RID INT,
	@LAYER_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM BULK_COLOR_RULE_LAYER
    WHERE HDR_BC_RID = @HDR_BC_RID
          AND LAYER_ID = @LAYER_ID

	SELECT @@ROWCOUNT;
END
GO