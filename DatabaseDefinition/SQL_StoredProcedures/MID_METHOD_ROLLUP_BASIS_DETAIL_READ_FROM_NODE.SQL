--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Reads rollup method basis details from the node, version and date range
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_NODE] 
	@HN_RID int,
	@FV_RID int,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM METHOD_ROLLUP MR, 
		 METHOD_ROLLUP_BASIS_DETAIL MRBD
    WHERE MR.METHOD_RID = MRBD.METHOD_RID 
		  AND MR.HN_RID = @HN_RID
		  AND MR.FV_RID = @FV_RID
		  AND MR.CDR_RID = @CDR_RID
    ORDER BY MR.HN_RID, MR.FV_RID, MR.CDR_RID

END
GO