--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads a Detal Work Override LL Model Name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME]
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT NAME
    FROM OVERRIDE_LL_MODEL_HEADER_WORK 
    WHERE OLL_RID = @OLL_RID
END
GO



