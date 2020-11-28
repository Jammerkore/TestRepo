--dv =============================================
--dv Modified:	3/7/2014
--dv Description:	Deletes and then inserts bulk rules
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_XML_BULK_RULE_WRITE] 
	@HDR_RID INT,
	@LAYER_ID INT,
	@RULE_TABLE BULK_RULE_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON

	-- Delete previous records
	DELETE
	FROM BULK_RULE
	WHERE HDR_RID = @HDR_RID
		AND LAYER_ID = @LAYER_ID

	-- Process records
	INSERT BULK_RULE (
		HDR_RID,
		LAYER_ID,
		ST_RID,
		RULE_TYPE_ID,
		UNITS
		)
	SELECT @HDR_RID,
		@LAYER_ID,
		ST_RID,
		RULE_TYPE,
		UNITS
	FROM @RULE_TABLE

	SELECT @@ROWCOUNT;

END
GO


