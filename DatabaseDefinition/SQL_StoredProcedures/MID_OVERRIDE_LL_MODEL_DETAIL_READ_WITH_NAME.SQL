--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads all records Header Work Override LL Model 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME]
	@NAME varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		OLL_RID
    FROM OVERRIDE_LL_MODEL_HEADER
    WHERE NAME = @NAME

END
GO