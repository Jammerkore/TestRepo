--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads top <x> number of rows from the color node table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_NODE_READ_TOP]
	@ROWS_TO_RETURN int
AS
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

    SELECT TOP (@ROWS_TO_RETURN) 
		cn.HN_RID, 
		cn.STYLE_NODE_ID, 
		cc.COLOR_CODE_ID 
    FROM COLOR_NODE cn, 
		 COLOR_CODE cc 
    WHERE cn.COLOR_CODE_RID = cc.COLOR_CODE_RID

    SET TRANSACTION ISOLATION LEVEL READ COMMITTED

END
GO
