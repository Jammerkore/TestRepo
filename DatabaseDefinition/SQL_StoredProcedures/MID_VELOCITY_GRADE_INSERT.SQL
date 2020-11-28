--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row into the velocity grade table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VELOCITY_GRADE_INSERT]
	@HN_RID int,
	@BOUNDARY int,
	@GRADE_CODE varchar(20)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO VELOCITY_GRADE(HN_RID, BOUNDARY, GRADE_CODE)
	VALUES (@HN_RID, @BOUNDARY, @GRADE_CODE)

	SELECT @@ROWCOUNT;

END
GO
