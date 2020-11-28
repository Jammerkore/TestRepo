--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Inserts a bulk rule layer
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_RULE_LAYER_INSERT]
	@HDR_RID INT,
	@LAYER_ID INT,
	@METHOD_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO BULK_RULE_LAYER
		( 
		HDR_RID, 
		LAYER_ID, 
		METHOD_RID 
		)
	VALUES
		( 
		@HDR_RID, 
		@LAYER_ID, 
		@METHOD_RID 
		)
	
	SELECT @@ROWCOUNT;
END
GO