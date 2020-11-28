--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Counts number of items for a computation group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_ITEM_READ_COUNT]
	@CG_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM COMPUTATION_ITEM WITH (NOLOCK)
	WHERE CG_RID = @CG_RID

END
GO
