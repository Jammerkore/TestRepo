--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads all linked headers that are not in use by multi-headers
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_LINKED] 
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT h.HDR_RID, h.HDR_ID
    FROM VW_GET_HEADERS h, 
		 HEADER_CHAR_JOIN hcj
    WHERE hcj.HC_RID = (SELECT hc2.HC_RID
                        FROM HEADER_CHAR hc2, HEADER_CHAR_JOIN hcj2, SYSTEM_OPTIONS so
                    	WHERE hcj2.HDR_RID = @HDR_RID
                   		      AND hc2.HC_RID = hcj2.HC_RID
                   		      AND hc2.HCG_RID = so.HEADER_LINK_CHARACTERISTIC
					   )
          AND h.HDR_RID = hcj.HDR_RID
          AND h.InUse = '0' 

END
GO
