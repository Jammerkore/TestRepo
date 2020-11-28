--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads filtered override low level header models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED]
	@NAME_FILTER varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		OLL_RID as MODEL_RID, 
		NAME as MODEL_NAME 
	FROM OVERRIDE_LL_MODEL_HEADER
    WHERE NAME LIKE @NAME_FILTER
	ORDER BY NAME

END
GO