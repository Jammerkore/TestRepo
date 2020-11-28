--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Deletes a size constraint group level from the size constraint provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CONSTRAINT_GRPLVL_DELETE]
	@SIZE_CONSTRAINT_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_CONSTRAINT_GRPLVL 
	WHERE SIZE_CONSTRAINT_RID = @SIZE_CONSTRAINT_RID

	SELECT @@ROWCOUNT;
END
GO