--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Returns the most recent 
--dv				Assortment Store Summary
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STORE_SUMMARY_READ]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		TOP 1 ass.*
    FROM ASSORTMENT_STORE_SUMMARY ass
    
END
GO