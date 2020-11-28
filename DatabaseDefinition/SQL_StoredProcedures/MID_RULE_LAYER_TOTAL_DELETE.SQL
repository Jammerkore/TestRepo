--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes the total rule layer associated with the header passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_RULE_LAYER_TOTAL_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM TOTAL_RULE_LAYER WITH (ROWLOCK) 
	WHERE HDR_RID=@HDR_RID

	SELECT @@ROWCOUNT;

END
GO
