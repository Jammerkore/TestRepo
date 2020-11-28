--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads header group characteristics
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_GROUP_READ] 
	@HCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		HCG_RID, 
		HCG_ID, 
		HCG_TYPE, 
		HCG_LIST_IND, 
		HCG_PROTECT_IND 
	FROM HEADER_CHAR_GROUP
	WHERE HCG_RID = @HCG_RID

END
GO