--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads filtered override low level header models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_READ_FILTERED_CASE_INSENSITIVE]
	@NAME_FILTER varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		OLL_RID as MODEL_RID, 
		NAME as MODEL_NAME 
	FROM OVERRIDE_LL_MODEL_HEADER
    WHERE NAME LIKE @NAME_FILTER collate SQL_Latin1_General_CP1_CI_AS --case insensitive
	ORDER BY NAME

END
GO