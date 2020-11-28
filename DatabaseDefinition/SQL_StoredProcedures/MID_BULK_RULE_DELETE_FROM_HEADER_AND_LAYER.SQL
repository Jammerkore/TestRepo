--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Deletes the rules for the header and layer
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_RULE_DELETE_FROM_HEADER_AND_LAYER]
	@HDR_RID INT,
	@LAYER_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM BULK_RULE 
	WHERE HDR_RID = @HDR_RID
		  AND LAYER_ID = @LAYER_ID

	SELECT @@ROWCOUNT;
END
GO

