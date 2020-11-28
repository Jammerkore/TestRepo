--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes the bulk color rule layer associated with the header bulk color rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_RULE_LAYER_BULK_COLOR_DELETE]
	@HDR_BC_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM BULK_COLOR_RULE_LAYER WITH (ROWLOCK) 
	WHERE HDR_BC_RID = @HDR_BC_RID

	SELECT @@ROWCOUNT;

END
GO
