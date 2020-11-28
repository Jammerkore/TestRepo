--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Deletes an Override LL Model Detail Work
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE]
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM OVERRIDE_LL_MODEL_DETAIL_WORK  
	WHERE OLL_RID = @OLL_RID

	SELECT @@ROWCOUNT;
END
GO