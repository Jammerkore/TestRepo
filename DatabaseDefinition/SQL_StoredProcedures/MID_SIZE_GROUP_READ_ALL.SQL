--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Reads all size groups
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_GROUP_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SIZE_GROUP_RID, 
		SIZE_GROUP_NAME, 
		SIZE_GROUP_DESCRIPTION, 
		WIDTH_ACROSS_IND 
	FROM SIZE_GROUP
    ORDER BY SIZE_GROUP_NAME

END
GO


