--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Updates a pack rule for the header, layer and store
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PACK_RULE_UPDATE]
	@HDR_PACK_RID INT,
	@LAYER_ID INT,
	@ST_RID INT,
	@RULE_TYPE_ID INT,
	@PACKS INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE PACK_RULE
	SET
		RULE_TYPE_ID = @RULE_TYPE_ID, 
		PACKS = @PACKS
	WHERE HDR_PACK_RID = @HDR_PACK_RID 
		  AND LAYER_ID = @LAYER_ID 
		  AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;
END
GO