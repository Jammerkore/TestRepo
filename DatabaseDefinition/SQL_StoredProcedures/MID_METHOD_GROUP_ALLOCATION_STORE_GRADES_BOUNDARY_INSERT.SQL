--dv =============================================
--dv Create date:	8/14/2014
--dv Description:	Inserts a group allocation method store grade boundary
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_INSERT]
	@METHOD_RID int,
	@BOUNDARY int, 
	@GRADE_CODE varchar(20)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY 
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