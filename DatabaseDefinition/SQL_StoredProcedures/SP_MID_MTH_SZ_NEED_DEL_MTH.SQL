CREATE PROCEDURE [dbo].[SP_MID_MTH_SZ_NEED_DEL_MTH] 
	(@METHODRID INT)
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON

	/****************************************************************************************/
	/* CREATED: 9/16/2004 - JEFF SCHNIERLE                                                  */
	/* MODIFIED: 04/19/2005 - STEVE TODD							*/
	/* PURPOSE: DELETES SIZE NEED METHOD AND ALL CHILD DATA		                            */
	/* EXEC SP_MID_MTH_SZ_NEED_DEL_MTH 296							*/
	/****************************************************************************************/
	EXEC SP_MID_MTH_RULE_DEL_CHILDREN @METHODRID, 0

	DELETE
	FROM METHOD_SIZE_NEED
	WHERE METHOD_RID = @METHODRID

	SELECT @@ROWCOUNT;

	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


