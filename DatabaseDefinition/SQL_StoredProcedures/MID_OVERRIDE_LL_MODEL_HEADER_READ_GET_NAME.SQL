--dv =============================================
--dv Create date:	01/30/2014
--dv Description:	Gets the Name of the Header Override LL Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME]
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		NAME
    FROM OVERRIDE_LL_MODEL_HEADER
    WHERE OLL_RID = @OLL_RID

END
GO