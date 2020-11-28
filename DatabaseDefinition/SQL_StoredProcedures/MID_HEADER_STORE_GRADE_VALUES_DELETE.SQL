--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes a row from the header store grade values table based on the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_STORE_GRADE_VALUES_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE HEADER_STORE_GRADE_VALUES WITH (ROWLOCK) 
    WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;

END
GO
