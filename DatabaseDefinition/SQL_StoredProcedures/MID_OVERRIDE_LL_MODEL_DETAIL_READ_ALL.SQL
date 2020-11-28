--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads all records Header Work Override LL Model 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL]
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ollmd.*
    FROM OVERRIDE_LL_MODEL_DETAIL ollmd
    WHERE ollmd.OLL_RID = @OLL_RID

END
GO