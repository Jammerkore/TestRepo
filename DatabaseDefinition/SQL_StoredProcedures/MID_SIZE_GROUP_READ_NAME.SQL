--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Reads a size group name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_GROUP_READ_NAME]
	@SIZE_GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SIZE_GROUP_NAME
    FROM SIZE_GROUP SG 
    WHERE SIZE_GROUP_RID = @SIZE_GROUP_RID

END
GO
