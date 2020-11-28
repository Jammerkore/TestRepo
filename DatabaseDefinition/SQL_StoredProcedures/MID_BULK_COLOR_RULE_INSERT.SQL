--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Inserts a bulk color rule for the header, layer and store
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_COLOR_RULE_INSERT]
	@HDR_RID INT,
	@HDR_BC_RID INT,
	@LAYER_ID INT,
	@ST_RID INT,
	@RULE_TYPE_ID INT,
	@UNITS INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO BULK_COLOR_RULE
		(
		HDR_RID,
		HDR_BC_RID, 
		LAYER_ID, 
		ST_RID, 
		RULE_TYPE_ID, 
		UNITS
		)
	VALUES
		(
		@HDR_RID,
		@HDR_BC_RID, 
		@LAYER_ID, 
		@ST_RID, 
		@RULE_TYPE_ID, 
		@UNITS
		)

	SELECT @@ROWCOUNT;
END
GO