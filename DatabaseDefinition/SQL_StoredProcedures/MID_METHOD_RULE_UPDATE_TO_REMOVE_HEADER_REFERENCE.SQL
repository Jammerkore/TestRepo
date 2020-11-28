--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Updates the method rule to remove reference to a header passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_RULE_UPDATE_TO_REMOVE_HEADER_REFERENCE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE METHOD_RULE 
	SET
		METHOD_RULE.HDR_RID = null,
		METHOD_RULE.HEADER_COMPONENT = null, 
		METHOD_RULE.HDR_PACK_RID = null,
        METHOD_RULE.HDR_BC_RID = null
	FROM METHOD 
	INNER JOIN METHOD_RULE ON METHOD.METHOD_RID = METHOD.METHOD_RID
	WHERE METHOD_RULE.HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;

END
GO
