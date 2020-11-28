--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes the pack rule layer associated with the header pack rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_RULE_LAYER_PACK_DELETE]
	@HDR_PACK_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PACK_RULE_LAYER WITH (ROWLOCK) 
	WHERE HDR_PACK_RID=@HDR_PACK_RID

	SELECT @@ROWCOUNT;

END
GO
