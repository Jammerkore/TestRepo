CREATE PROCEDURE [dbo].[SP_MID_MTH_SZ_CURVE_DEL_MTH] 
	(@METHOD_RID INT)
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON

	/****************************************************************************************/
	/* CREATED: 5/28/2009 - JOHN SMITH                                                      */
	/* PURPOSE: DELETES SIZE CURVE METHOD AND ALL CHILD DATA	                            */
	/****************************************************************************************/
	DELETE
	FROM METHOD_SIZE_CURVE_MRCH_BAS_DET
	WHERE METHOD_RID = @METHOD_RID

	DELETE
	FROM METHOD_SIZE_CURVE_CRVE_BAS_DET
	WHERE METHOD_RID = @METHOD_RID

	DELETE
	FROM METHOD_SIZE_CURVE
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


