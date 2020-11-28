--dv =============================================
--dv Modified date:	8/14/2014
--dv Description:	Gets headers that can be deleted
--dv History:		TT#1268 - MD - RMatelic - 5.4 Merge
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_GET_HEADERS_TO_DELETE] 
AS
BEGIN
	SELECT hdr.HDR_RID,
		hdr.HDR_ID,
		hdr.ALLOCATION_STATUS_FLAGS,
		hdr.SHIPPING_STATUS_FLAGS
	FROM HEADER hdr
	JOIN VW_GET_HEADERS vw ON vw.HDR_RID = hdr.HDR_RID
	JOIN PURGE_DATES pd ON hdr.STYLE_HNRID = pd.HN_RID
	WHERE vw.ShippingComplete = '1'
		AND vw.MultiHeader = 0
		AND vw.HDR_GROUP_RID = 1
		AND vw.Assortment = 0		-- not assortment/GA headers						 -- TT#1091-MD - STodd - Group Allocation Purge  
	    AND vw.Placeholder = 0		-- not a placeholder								 -- TT#1091-MD - STodd - Group Allocation Purge  
	    AND vw.ASRT_RID is null		-- not headers that belong to an assortment/GA	
		AND (
			(
				vw.Receipt = 1
				AND hdr.RELEASE_DATETIME < pd.PURGE_HEADERS_RECEIPT_DATETIME
				)
			OR (
				vw.ASN = 1
				AND hdr.RELEASE_DATETIME < pd.PURGE_HEADERS_ASN_DATETIME
				)
			OR (
				vw.Dummy = 1
				AND hdr.RELEASE_DATETIME < pd.PURGE_HEADERS_DUMMY_DATETIME
				)
			OR (
				vw.DropShip = 1
				AND hdr.RELEASE_DATETIME < pd.PURGE_HEADERS_DROPSHIP_DATETIME
				)
			OR (
				vw.Reserve = 1
				AND hdr.RELEASE_DATETIME < pd.PURGE_HEADERS_RESERVE_DATETIME
				)
			OR (
				vw.WorkUpTotalBuy = 1
				AND hdr.RELEASE_DATETIME < pd.PURGE_HEADERS_WORKUPTOTALBUY_DATETIME
				)
			OR (
				vw.PurchaseOrder = 1
				AND hdr.RELEASE_DATETIME < pd.PURGE_HEADERS_PO_DATETIME
				)
			OR (
				vw.VSWHeader = 1
				AND hdr.RELEASE_DATETIME < pd.PURGE_HEADERS_VSW_DATETIME
				)
			)
END
GO


