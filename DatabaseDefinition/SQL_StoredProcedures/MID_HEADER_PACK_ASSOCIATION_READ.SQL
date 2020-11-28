--dv =============================================
--dv Create date:	5/8/2017
--dv Description:	Gets header packs associations for the specified header RID
--dv History: TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_ASSOCIATION_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	--SELECT * 
	--FROM HEADER_PACK_ASSOCIATION 
	--WHERE HDR_RID = @HDR_RID
	SELECT hpa.*, hp.HDR_PACK_NAME 
	FROM HEADER_PACK_ASSOCIATION hpa 
	join HEADER_PACK hp with (nolock) on hp.HDR_PACK_RID = hpa.ASSOCIATED_PACK_RID
	WHERE hpa.HDR_RID = @HDR_RID

END
GO
