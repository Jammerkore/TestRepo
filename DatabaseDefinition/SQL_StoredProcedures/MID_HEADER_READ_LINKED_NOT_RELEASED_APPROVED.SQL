--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads linked headers that are not released approved
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_LINKED_NOT_RELEASED_APPROVED] 
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT h.HDR_RID, h.HDR_ID
    FROM VW_GET_HEADERS h, 
	     HEADER_CHAR_JOIN hcj
    WHERE hcj.HC_RID = (SELECT hc.HC_RID
                    	FROM SYSTEM_OPTIONS so, 
							 HEADER_CHAR hc, 
							 HEADER_CHAR_JOIN hcj
                     	WHERE hcj.HDR_RID = @HDR_RID
                    		  AND hc.HC_RID = hcj.HC_RID
                    		  AND hc.HCG_RID = so.HEADER_LINK_CHARACTERISTIC
					    )
          AND h.HDR_RID = hcj.HDR_RID
          AND h.ReleaseApproved = '0'  

END
GO
