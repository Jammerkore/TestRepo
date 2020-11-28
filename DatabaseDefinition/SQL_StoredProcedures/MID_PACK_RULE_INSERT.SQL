--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Inserts a pack rule for the header, layer and store
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PACK_RULE_INSERT]
	@HDR_PACK_RID INT,
	@LAYER_ID INT,
	@ST_RID INT,
	@RULE_TYPE_ID INT,
	@PACKS INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PACK_RULE
		(
		HDR_PACK_RID, 
		LAYER_ID, 
		ST_RID, 
		RULE_TYPE_ID, 
		PACKS
		)
	VALUES
		(
		@HDR_PACK_RID, 
		@LAYER_ID, 
		@ST_RID, 
		@RULE_TYPE_ID, 
		@PACKS
		)

	SELECT @@ROWCOUNT;
END
GO