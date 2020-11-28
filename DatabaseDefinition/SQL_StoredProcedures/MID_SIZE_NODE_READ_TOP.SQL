--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads top <x> number of rows from the size node table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_NODE_READ_TOP]
	@ROWS_TO_RETURN int
AS
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

	SELECT TOP (@ROWS_TO_RETURN) 
		sn.HN_RID, 
		sn.STYLE_NODE_ID, 
		sn.COLOR_NODE_ID, 
		sc.SIZE_CODE_ID 
	FROM SIZE_NODE sn, 
		 SIZE_CODE sc 
	WHERE sn.SIZE_CODE_RID = sc.SIZE_CODE_RID
			
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED

END
GO
