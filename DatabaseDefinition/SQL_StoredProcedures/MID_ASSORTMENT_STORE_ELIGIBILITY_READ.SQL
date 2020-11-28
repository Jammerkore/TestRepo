--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Returns all Assortment Eligible Stores
--dv				for a Header 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STORE_ELIGIBILITY_READ]
	@HDR_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		*
    FROM ASSORTMENT_STORE_ELIGIBILITY 
	WHERE HDR_RID = @HDR_RID
	ORDER BY ST_RID
	
END
GO