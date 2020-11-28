--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Updates a bulk color rule for the header, layer and store
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_COLOR_RULE_UPDATE]
	@HDR_BC_RID INT,
	@LAYER_ID INT,
	@ST_RID INT,
	@RULE_TYPE_ID INT,
	@UNITS INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE BULK_COLOR_RULE
	SET
		RULE_TYPE_ID = @RULE_TYPE_ID, 
		UNITS = @UNITS
	WHERE HDR_BC_RID = @HDR_BC_RID 
		  AND LAYER_ID = @LAYER_ID 
		  AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;
END
GO