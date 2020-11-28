CREATE PROCEDURE [dbo].[SP_MID_MTH_SIZE_BASIS_DEL] 
	(@METHODRID INT)
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON

	/*****************************************************************************************/
	/* CREATED: 8/19/2004 - JEFF SCHNIERLE                                                   */
	/* PURPOSE: DELETES BASIS SIZE METHOD AND ALL CHILD DATA                                 */
	/* EXEC SP_MID_MTH_SIZE_BASIS_DEL 296 			                                 */
	/*****************************************************************************************/
	EXEC SP_MID_MTH_RULE_DEL_CHILDREN @METHODRID, 0

	DELETE
	FROM METHOD_SIZE_BASIS_SUBSTITUTES
	WHERE METHOD_RID = @METHODRID

	DELETE
	FROM METHOD_SIZE_BASIS_ALLOCATION
	WHERE METHOD_RID = @METHODRID

	SELECT @@ROWCOUNT;

	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


