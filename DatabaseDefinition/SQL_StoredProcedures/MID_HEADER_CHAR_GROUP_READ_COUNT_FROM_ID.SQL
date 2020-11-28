--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads the count of header group characteristics, based on the ID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_GROUP_READ_COUNT_FROM_ID] 
	@HCG_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		COUNT(*) MyCount 
	FROM HEADER_CHAR_GROUP 
	WHERE HCG_ID = @HCG_ID

END
GO