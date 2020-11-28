--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Inserts a modify sales method grade
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_GRADE_INSERT] 
	@METHOD_RID int,
	@BOUNDARY int,
	@GRADE_CODE varchar(20)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_MOD_SALES_GRADE
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
