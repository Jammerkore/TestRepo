--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Deletes a Work Override LL Model
--dv History: 6/24/2014 -TT#1233-MD -jsobek -Override Low Level Model Unhandled Exception
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE]
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM OVERRIDE_LL_MODEL_HEADER_WORK  
	WHERE
	OLL_RID = @OLL_RID
	 
	SELECT @@ROWCOUNT;
END
GO