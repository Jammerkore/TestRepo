--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads the count of the posting date based on the product hierarchy rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_POSTING_DATE_READ_COUNT] 
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM POSTING_DATE 
	WHERE PH_RID = @PH_RID

END
GO
