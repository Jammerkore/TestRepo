--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Returns all Assortment Store Summary 
--dv				records for a header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STYLE_CLOSED_READ_HDR_LIST]
	@HDR_RID_LIST HDR_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		*
    FROM ASSORTMENT_STYLE_CLOSED 
	WHERE HDR_RID in (SELECT HDR_RID FROM @HDR_RID_LIST)
	
END
GO