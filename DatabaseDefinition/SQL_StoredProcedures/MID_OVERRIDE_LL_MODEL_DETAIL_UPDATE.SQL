--dv =============================================
--dv Create date:	11/29/2013
--dv Description:	Updates a Header Work Override LL Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE]
	@OLL_RID int,
	@HN_RID int,
	@VERSION_RID int,
	@EXCLUDE_IND char(1)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE OVERRIDE_LL_MODEL_DETAIL 
	SET  
		HN_RID = @HN_RID,
		VERSION_RID = @VERSION_RID,
		EXCLUDE_IND = @EXCLUDE_IND
	WHERE OLL_RID = @OLL_RID 
	AND HN_RID = @HN_RID
	SELECT @@ROWCOUNT;
END
GO