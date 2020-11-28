--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads all header group characteristics
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_GROUP_READ_ALL] 
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
	ORDER BY HCG_ID

END
GO