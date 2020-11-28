--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Inserts a grade boundary for the overrride method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_INSERT]
	@METHOD_RID int, 
	@BOUNDARY int, 
	@GRADE_CODE varchar(20)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_OVERRIDE_STORE_GRADES_BOUNDARY 
		(
		METHOD_RID, 
		BOUNDARY, 
		GRADE_CODE
		)
	VALUES
		(
		@METHOD_RID, 
		@BOUNDARY, 
		@GRADE_CODE
		)

	SELECT @@ROWCOUNT;
END
GO