--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes the bulk rule layer associated with the header passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_RULE_LAYER_BULK_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM BULK_RULE_LAYER WITH (ROWLOCK) 
	WHERE HDR_RID=@HDR_RID

	SELECT @@ROWCOUNT;

END
GO
