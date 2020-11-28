--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads a Detal Work Override LL Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ]
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
    FROM OVERRIDE_LL_MODEL_DETAIL_WORK 
    WHERE OLL_RID = @OLL_RID
END
GO