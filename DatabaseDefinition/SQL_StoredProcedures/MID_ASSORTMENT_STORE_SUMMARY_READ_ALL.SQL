--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Returns all Assortment Store Summary 
--dv				records for a header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STORE_SUMMARY_READ_ALL]
	@HDR_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ass.*
    FROM ASSORTMENT_STORE_SUMMARY ass
	WHERE ass.HDR_RID = @HDR_RID
	ORDER BY ass.VARIABLE_NUMBER
    
END
GO