CREATE PROCEDURE [dbo].[SP_MID_MTH_RULE_DEL_CHILDREN] 
	(@METHODRID INT, 
	@RETURN_ROWCOUNT BIT = 1)
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON

	/*****************************************************************************************/
	/* CREATED: 4/19/2005 - Steve Todd                                                   */
	/* PURPOSE: DELETES THE CHILD DATA OF A SIZE METHOD		                         */
	/* EXEC SP_MID_MTH_RULE_DEL_CHILDREN 303                                                */
	/*****************************************************************************************/
	--MINMAX MUST BE DELETED FIRST DUE TO CONSTRAINTS
	DELETE
	FROM SIZE_MTH_RULE_COLOR_SIZE
	WHERE METHOD_RID = @METHODRID

	DELETE
	FROM SIZE_MTH_RULE_GRPLVL
	WHERE METHOD_RID = @METHODRID

	IF @RETURN_ROWCOUNT = 1 SELECT @@ROWCOUNT;

	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


