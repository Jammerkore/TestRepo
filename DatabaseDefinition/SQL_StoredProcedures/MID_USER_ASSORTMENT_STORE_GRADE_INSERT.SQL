--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Inserts Assortment User Store Grades
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ASSORTMENT_STORE_GRADE_INSERT]
	@USER_RID int, 
	@STORE_GRADE_SEQ int, 
	@BOUNDARY_UNITS int, 
	@BOUNDARY_INDEX int, 
	@GRADE_CODE varchar(20)
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO USER_ASSORTMENT_STORE_GRADE
		(
		USER_RID, 
		STORE_GRADE_SEQ, 
		BOUNDARY_UNITS, 
		BOUNDARY_INDEX, 
		GRADE_CODE
		)		
	VALUES
		(
		@USER_RID, 
		@STORE_GRADE_SEQ, 
		@BOUNDARY_UNITS, 
		@BOUNDARY_INDEX, 
		@GRADE_CODE
		)		

    SELECT @@ROWCOUNT;
END

GO