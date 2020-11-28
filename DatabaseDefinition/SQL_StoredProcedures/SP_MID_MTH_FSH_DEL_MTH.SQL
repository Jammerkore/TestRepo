CREATE PROCEDURE [dbo].[SP_MID_MTH_FSH_DEL_MTH] 
	(@METHODRID INT)
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON

	/*****************************************************************************************/
	/* CREATED: 7/05/2004 - JEFF SCHNIERLE                                                   */
	/* PURPOSE: DELETES FILL SIZE HOLE METHOD AND ALL CHILD DATA                             */
	/* EXEC SP_MID_MTH_FSH_DEL_MTH 296 			                                 */
	/*****************************************************************************************/
	EXEC SP_MID_MTH_RULE_DEL_CHILDREN @METHODRID, 0

	DELETE
	FROM METHOD_FILL_SIZE_HOLES
	WHERE METHOD_RID = @METHODRID

	SELECT @@ROWCOUNT;

	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


