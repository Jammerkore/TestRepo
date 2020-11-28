--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads size ids based on the style and color ids passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_NODE_READ_IDS_FROM_STYLE_AND_COLOR]
	@STYLE_NODE_ID varchar(50),
	@COLOR_NODE_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

	SELECT 
		sn.HN_RID, 
		sn.STYLE_NODE_ID, 
		sn.COLOR_NODE_ID, 
		sc.SIZE_CODE_ID 
	FROM SIZE_NODE sn, 
		 SIZE_CODE sc 
	WHERE sn.COLOR_NODE_ID = @COLOR_NODE_ID
		  AND sn.STYLE_NODE_ID = @STYLE_NODE_ID
		  AND sn.SIZE_CODE_RID = sc.SIZE_CODE_RID
			
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED

END
GO
