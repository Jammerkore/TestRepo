--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Reads rows from the method copy basis detail table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_COPY_BASIS_DETAIL_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		SGL_RID, 
		DETAIL_SEQ, 
		' ' AS Merchandise,
		COALESCE(HN_RID, -1) HN_RID,
		FV_RID, 
		' ' AS DateRange, 
		CDR_RID, 
		WEIGHT, 
		INCLUDE_EXCLUDE, 
		' ' AS IncludeButton
	FROM METHOD_COPY_BASIS_DETAIL
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY DETAIL_SEQ

END
GO