--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Deletes a Work Override LL Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_DELETE]
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM OVERRIDE_LL_MODEL_HEADER  
	WHERE
	OLL_RID = @OLL_RID 
	 
	SELECT @@ROWCOUNT;
END
GO