--dv =============================================
--dv Create date:	11/30/2013
--dv Description:	Inserts a Copy of a New Detail Work Override LL Model 
--dv into the Detail Override LL Model table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL]
	@OLL_RID int,
	@NEW_OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO OVERRIDE_LL_MODEL_DETAIL  
		(
		OLL_RID,
		HN_RID,
		VERSION_RID,
		EXCLUDE_IND
		)
		SELECT 
		@NEW_OLL_RID,
		HN_RID,
		VERSION_RID,
		EXCLUDE_IND
		FROM OVERRIDE_LL_MODEL_DETAIL_WORK
        WHERE OLL_RID = @OLL_RID

	SELECT @@ROWCOUNT;
END
GO





