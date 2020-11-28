--dv =============================================
--dv Create date:	1/17/2014
--dv Description:	Reads header group characteristics from header characteristic group id
--dv				Case insensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_GROUP_READ_FROM_ID]
	@HCG_ID varchar(50)
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
	WHERE HCG_ID = @HCG_ID collate SQL_Latin1_General_CP1_CI_AS --case insensitive

END
GO