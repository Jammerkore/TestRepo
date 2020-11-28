--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes all rows from the velocity grade table passed on the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VELOCITY_GRADE_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM VELOCITY_GRADE WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
