CREATE PROCEDURE [dbo].[SP_MID_SIZE_CONST_GL_INSERT] 
	(
	@SIZECONSTRAINTRID INT,
	@SGLRID INT,
	@MIN INT = NULL,
	@MAX INT = NULL,
	@MULT INT = NULL,
	@ROWTYPEID INT
	)
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON

	/*****************************************************************************************/
	/* CREATED: 7/01/2004 - JEFF SCHNIERLE  						 */
	/* MODIFIED: 03/09/2005 - Steve Todd for Size COnstraints                                */
	/* PURPOSE: INSERTS DATA IN SIZE_CONSTRAINT_GRPLVL		                 */
	/*****************************************************************************************/
	BEGIN
		INSERT INTO SIZE_CONSTRAINT_GRPLVL (
			ROW_TYPE_ID,
			SIZE_CONSTRAINT_RID,
			SGL_RID,
			SIZE_MIN,
			SIZE_MAX,
			SIZE_MULT
			)
		VALUES (
			@ROWTYPEID,
			@SIZECONSTRAINTRID,
			@SGLRID,
			@MIN,
			@MAX,
			@MULT
			)
	END
	SELECT @@ROWCOUNT;
	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


