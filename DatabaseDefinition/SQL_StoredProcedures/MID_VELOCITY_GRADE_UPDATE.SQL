--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Updates a row in the velocity grade table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VELOCITY_GRADE_UPDATE]
	@HN_RID int,
	@BOUNDARY int,
	@GRADE_CODE varchar(20)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE VELOCITY_GRADE WITH (ROWLOCK) 
	SET
		GRADE_CODE = @GRADE_CODE
	WHERE HN_RID = @HN_RID
		  AND BOUNDARY = @BOUNDARY
	
	SELECT @@ROWCOUNT;

END
GO
