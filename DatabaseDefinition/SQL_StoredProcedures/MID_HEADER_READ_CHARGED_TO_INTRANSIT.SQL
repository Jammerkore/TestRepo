--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Reads headers that have been charged to Instransit for the Rebuild Intransit Utility
--dv History:		8/14/2014 TT#488 - MD - JEllis - Group Allocation
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_CHARGED_TO_INTRANSIT]
	@HN_RID_LIST HN_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_RID, 
		STYLE_HNRID, 
		SHIPPING_STATUS_FLAGS, 
		INTRANSIT_STATUS_FLAGS,
		ALLOCATION_TYPE_FLAGS
	FROM HEADER
	WHERE INTRANSIT_STATUS_FLAGS > 0 
		  AND STYLE_HNRID IN (SELECT HN_RID FROM @HN_RID_LIST)

END
GO
