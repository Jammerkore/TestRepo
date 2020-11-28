--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row from the eligibility model table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ELIGIBILITY_MODEL_DELETE]
	@EM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM ELIGIBILITY_MODEL WITH (ROWLOCK)
	WHERE EM_RID = @EM_RID

	SELECT @@ROWCOUNT;

END
GO
