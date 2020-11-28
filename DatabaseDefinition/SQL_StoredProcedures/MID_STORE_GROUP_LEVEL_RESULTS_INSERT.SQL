--dv =============================================
--dv Create date: 4/10/2015
--dv Description:	Inserts a store group level results entry
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_LEVEL_RESULTS_INSERT] 
	@SGL_RID INT,
	@SGL_VERSION INT,
	@ST_RID INT,
	@RESULT_RID INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO STORE_GROUP_LEVEL_RESULTS
		(
		SGL_RID,
		SGL_VERSION,
		ST_RID
		)
	VALUES 
		(
		@SGL_RID,
		@SGL_VERSION,
		@ST_RID
		)

	SELECT @RESULT_RID = SCOPE_IDENTITY();
END
GO


