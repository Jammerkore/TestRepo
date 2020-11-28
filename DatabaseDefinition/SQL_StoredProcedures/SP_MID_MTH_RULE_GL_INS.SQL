CREATE PROCEDURE [dbo].[SP_MID_MTH_RULE_GL_INS] 
	(
	@METHODRID INT,
	@SGLRID INT,
	@RULE INT = NULL,
	@QTY INT = NULL,
	@ROWTYPEID INT
	)
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON

	/*****************************************************************************************/
	/* CREATED: 3/31/2005 - STEVE TODD                                                   */
	/* PURPOSE: INSERTS OR UPDATES DATA IN SIZE_MTH_RULE_GRPLVL		                 */
	/*****************************************************************************************/
	/*UPDATE IS NOT IMPLEMENTED BECAUSE THE RECORDS ARE DELETED PRIOR TO SAVING, SO THE INSERT WOULD 
	  HAPPEN EACH TIME*/
	--if not exists (select * from METHOD_FILL_SIZE_HOLES_GRP_LVL WHERE METHOD_RID = @METHODRID AND SGL_RID = @SGLRID)
	--INSERT
	BEGIN
		INSERT INTO SIZE_MTH_RULE_GRPLVL (
			ROW_TYPE_ID,
			METHOD_RID,
			SGL_RID,
			SIZE_RULE,
			SIZE_QUANTITY
			)
		VALUES (
			@ROWTYPEID,
			@METHODRID,
			@SGLRID,
			@RULE,
			@QTY
			)
	END

	SELECT @@ROWCOUNT

	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


