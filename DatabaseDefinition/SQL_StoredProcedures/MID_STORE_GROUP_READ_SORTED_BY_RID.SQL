--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Reads store groups, sorted by rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_READ_SORTED_BY_RID]
	@SG_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GlobalUserRID INT = 4;

	SELECT 
		SG_RID, 
		SG_ID, 
		SG_DYNAMIC_GROUP_IND, 
		coalesce(USER_RID, @GlobalUserRID) USER_RID
    FROM STORE_GROUP 
	WHERE SG_RID = @SG_RID 
	ORDER BY SG_RID ASC

END
GO
