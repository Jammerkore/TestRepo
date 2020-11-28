--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads color ids based on the style hierarchy id passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_NODE_READ_FROM_STYLE]
	@STYLE_NODE_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

	SELECT 
		cn.HN_RID, 
		cn.STYLE_NODE_ID, 
		cc.COLOR_CODE_ID 
	FROM COLOR_NODE cn, 
		 COLOR_CODE cc 
	WHERE cn.STYLE_NODE_ID = @STYLE_NODE_ID
		  AND cn.COLOR_CODE_RID = cc.COLOR_CODE_RID
			
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED

END
GO
