--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads size sell thrus based on the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_SIZE_SELLTHRU_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		NSST.HN_RID, 
		NSST.SELLTHRU_LIMIT
	FROM NODE_SIZE_SELLTHRU NSST
	WHERE NSST.HN_RID = @HN_RID

END
GO
