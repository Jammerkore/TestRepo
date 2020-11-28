--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Updates the method rule status based on the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_RULE_UPDATE_STATUS]
	@HDR_RID int,
	@METHOD_STATUS int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
	METHOD WITH (ROWLOCK) 
	SET METHOD.METHOD_STATUS = @METHOD_STATUS
    FROM METHOD 
	INNER JOIN METHOD_RULE ON METHOD.METHOD_RID = METHOD.METHOD_RID
    WHERE METHOD_RULE.HDR_RID = @HDR_RID 
		  AND METHOD.METHOD_RID = METHOD_RULE.METHOD_RID

	SELECT @@ROWCOUNT;

END
GO
