--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Reads spread method basis details
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SPREAD_BASIS_DETAIL_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		DETAIL_SEQ,
		FV_RID, 
		' ' AS DateRange, 
		CDR_RID, 
		WEIGHT 
	FROM METHOD_SPREAD_BASIS_DETAIL 
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY DETAIL_SEQ

END
GO