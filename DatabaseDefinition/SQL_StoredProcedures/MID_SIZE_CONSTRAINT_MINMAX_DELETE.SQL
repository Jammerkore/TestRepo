--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Deletes a size constraint min max
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CONSTRAINT_MINMAX_DELETE]
	@SIZE_CONSTRAINT_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_CONSTRAINT_MINMAX 
	WHERE SIZE_CONSTRAINT_RID = @SIZE_CONSTRAINT_RID

	SELECT @@ROWCOUNT;
END
GO



