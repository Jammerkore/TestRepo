--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes a row from the header store grade table based on the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_STORE_GRADE_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE HEADER_STORE_GRADE WITH (ROWLOCK) 
    WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;

END
GO
