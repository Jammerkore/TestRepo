CREATE PROCEDURE [dbo].[SP_MID_HEADER_CHAR_DATA_TYPE] 
	(
	@CHARNAME VARCHAR(50),
	@HCG_TYPE INT OUTPUT
	)
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON
	/*****************************************************************************************/
	/* CREATED: 7/22/2004 - JEFF SCHNIERLE                                                   */
	/* PURPOSE: GETS THE DATA TYPE OF THE GROUP                                              */
	/* USAGE  :                                                                              */
	/* 	   DECLARE @DTYPE INT                                                            */
	/* 	   EXEC SP_MID_HEADER_CHAR_DATA_TYPE 'NAME', @DTYPE OUTPUT                       */
	/*****************************************************************************************/
	SET @HCG_TYPE = 4

	--RETRIEVE DATA TYPE FROM HEADER_CHAR_GROUP
	SELECT @HCG_TYPE = ISNULL(HCG_TYPE, 4)
	FROM HEADER_CHAR_GROUP
	WHERE HCG_ID = @CHARNAME

	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


