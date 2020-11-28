--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads all posting dates rids and dates
--dv =============================================
CREATE PROCEDURE [dbo].[MID_POSTING_DATE_RID_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		PH_RID, 
		POSTING_DATE 
	FROM POSTING_DATE	

END
GO
