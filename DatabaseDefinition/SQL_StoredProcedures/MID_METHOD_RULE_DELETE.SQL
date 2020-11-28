--dv =============================================
--dv Create date:	6/24/2014
--dv Description:	Deletes a rule method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_RULE_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_RULE
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO
