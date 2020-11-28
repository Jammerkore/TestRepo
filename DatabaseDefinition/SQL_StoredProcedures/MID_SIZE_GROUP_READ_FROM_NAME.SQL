--dv =============================================
--dv Create date:	1/17/2014
--dv Description:	Reads size groups from size group name
--dv				Case sensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_GROUP_READ_FROM_NAME]
	@SIZE_GROUP_NAME varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @UndefinedSizeGroupRID INT = 1;

    SELECT 
		SIZE_GROUP_RID, 
		SIZE_GROUP_NAME 
	FROM SIZE_GROUP
	WHERE SIZE_GROUP_RID <> @UndefinedSizeGroupRID
		AND SIZE_GROUP_NAME LIKE @SIZE_GROUP_NAME				
	ORDER BY SIZE_GROUP_NAME

END
GO