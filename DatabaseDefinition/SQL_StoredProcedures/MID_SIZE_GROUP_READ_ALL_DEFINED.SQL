--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Reads all defined size groups
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_GROUP_READ_ALL_DEFINED]
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @UndefinedSizeGroupRID INT = 1;

	SELECT 
		SIZE_GROUP_RID, 
		SIZE_GROUP_NAME, 
		SIZE_GROUP_DESCRIPTION, 
		WIDTH_ACROSS_IND 
	FROM SIZE_GROUP
    WHERE SIZE_GROUP_RID <> @UndefinedSizeGroupRID
    ORDER BY SIZE_GROUP_NAME

END
GO

