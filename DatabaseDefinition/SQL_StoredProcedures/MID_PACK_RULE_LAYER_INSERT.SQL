--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Inserts a pack rule layer
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PACK_RULE_LAYER_INSERT]
	@HDR_PACK_RID INT,
	@LAYER_ID INT,
	@METHOD_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PACK_RULE_LAYER
		( 
		HDR_PACK_RID, 
		LAYER_ID, 
		METHOD_RID 
		)
	VALUES
		( 
		@HDR_PACK_RID, 
		@LAYER_ID, 
		@METHOD_RID 
		)
	
	SELECT @@ROWCOUNT;
END
GO