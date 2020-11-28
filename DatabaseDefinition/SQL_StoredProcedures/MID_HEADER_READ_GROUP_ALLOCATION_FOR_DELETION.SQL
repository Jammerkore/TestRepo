--dv ================================================================
--dv Create date:	04/24/2014
--dv Description:	Finds the group allocations where the headers
--dv                within it all meet the header purge requirements.
--dv History:		TT#1091-MD - STodd - Group Allocation Purge
--dv =================================================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_GROUP_ALLOCATION_FOR_DELETION]
AS
BEGIN
	WITH cte_groupAllocations(HDR_RID, HDR_ID)
	AS (
		/* Find all groupo allocation headers */
		SELECT  HDR_RID, HDR_ID 
			FROM VW_GET_HEADERS
			where ASRT_TYPE in (3)
		)
		select cte.HDR_RID, cte.HDR_ID from cte_groupAllocations cte 
		where cte.HDR_RID not in
		/* Finds all headers within a group allocation that are not ready to purge */
		/* These group allocations are not ready to be purged		               */
			(select cte.HDR_RID 
			from  cte_groupAllocations cte  
			join  VW_GET_HEADERS vw on vw.ASRT_RID = cte.HDR_RID
			join PURGE_DATES pd on vw.STYLE_HNRID = pd.HN_RID
			where vw.Assortment = 0 and Placeholder = 0 and (vw.ShippingComplete = '0'
			   or ((vw.Receipt = 1 and vw.RELEASE_DATETIME !< pd.PURGE_HEADERS_RECEIPT_DATETIME)
			   or (vw.ASN = 1 and vw.RELEASE_DATETIME !< pd.PURGE_HEADERS_ASN_DATETIME)
			   or (vw.Dummy = 1 and vw.RELEASE_DATETIME !< pd.PURGE_HEADERS_DUMMY_DATETIME)
			   or (vw.DropShip = 1 and vw.RELEASE_DATETIME !< pd.PURGE_HEADERS_DROPSHIP_DATETIME)
			   or (vw.Reserve = 1 and vw.RELEASE_DATETIME !< pd.PURGE_HEADERS_RESERVE_DATETIME)
			   or (vw.WorkUpTotalBuy = 1 and vw.RELEASE_DATETIME !< pd.PURGE_HEADERS_WORKUPTOTALBUY_DATETIME)
			   or (vw.PurchaseOrder = 1 and vw.RELEASE_DATETIME !< pd.PURGE_HEADERS_PO_DATETIME)
			   or (vw.VSWHeader = 1 and vw.RELEASE_DATETIME !< pd.PURGE_HEADERS_VSW_DATETIME))))

END
GO