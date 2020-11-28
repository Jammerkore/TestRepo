--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row from the velocity grade table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VELOCITY_GRADE_DELETE]
	@HN_RID int,
	@BOUNDARY int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM VELOCITY_GRADE WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
		  AND BOUNDARY = @BOUNDARY
	
	SELECT @@ROWCOUNT;

END
GO
