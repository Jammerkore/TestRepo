--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes the detail rule layer associated with the header passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_RULE_LAYER_DETAIL_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM DETAIL_RULE_LAYER WITH (ROWLOCK) 
	WHERE HDR_RID=@HDR_RID

	SELECT @@ROWCOUNT;

END
GO
