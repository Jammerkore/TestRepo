--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts a Assortment Properties Store Grade
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_PROPERTIES_STORE_GRADE_INSERT]
	@HDR_RID int, 
	@STORE_GRADE_SEQ int, 
	@BOUNDARY_UNITS int, 
	@BOUNDARY_INDEX int, 
	@GRADE_CODE varchar(20)
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO ASSORTMENT_PROPERTIES_STORE_GRADE
		(
		HDR_RID, 
		STORE_GRADE_SEQ, 
		BOUNDARY_UNITS, 
		BOUNDARY_INDEX, 
		GRADE_CODE
		)		
	VALUES
		(
		@HDR_RID, 
		@STORE_GRADE_SEQ, 
		@BOUNDARY_UNITS, 
		@BOUNDARY_INDEX, 
		@GRADE_CODE
		)		

    SELECT @@ROWCOUNT;
END

GO