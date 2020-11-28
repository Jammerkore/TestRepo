--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Deletes a Work Override LL Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE]
	@OLL_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM OVERRIDE_LL_MODEL_DETAIL  
	WHERE
	OLL_RID = @OLL_RID 
	AND HN_RID = @HN_RID
	 
	SELECT @@ROWCOUNT;
END
GO