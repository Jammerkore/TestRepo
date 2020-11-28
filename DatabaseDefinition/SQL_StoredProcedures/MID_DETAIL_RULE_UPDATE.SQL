--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Updates a detail rule for the header, layer and store
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DETAIL_RULE_UPDATE]
	@HDR_RID INT,
	@LAYER_ID INT,
	@ST_RID INT,
	@RULE_TYPE_ID INT,
	@UNITS INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE DETAIL_RULE
	SET
		RULE_TYPE_ID = @RULE_TYPE_ID, 
		UNITS = @UNITS
	WHERE HDR_RID = @HDR_RID 
		  AND LAYER_ID = @LAYER_ID 
		  AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;
END
GO