--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads the components for the master header
--dv Modify:        10/19/2015 TT#1744-Data Layer Request - RMatelic - Add NOLOCK to MASTER_HEADER stored procedures.
--dv Modify:        11/02/2015 TT#1750-MD-Data Layer Request - RMatelic - Stored procedure is incorrectly returning SUBORD_HDR_ID instead of MASTER_HDR_ID as the HeaderServerSession expects.   
--dv =============================================
CREATE PROCEDURE [dbo].[MID_MASTER_HEADER_READ_COMPONENTS_FROM_SUBORD] 
	@SUBORD_HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @NO_RID int = -1;

	SELECT 
		COALESCE(SUBORD_HDR_RID, @NO_RID) AS SUBORD_HDR_RID,
        COALESCE(MASTER_HDR_RID, @NO_RID) AS MASTER_HDR_RID,
        MASTER_COMPONENT,
        COALESCE(MASTER_PACK_RID, @NO_RID) AS MASTER_PACK_RID,
        COALESCE(MASTER_BC_RID, @NO_RID) AS MASTER_BC_RID,
        SUBORD_COMPONENT,
        COALESCE(SUBORD_PACK_RID, @NO_RID) AS SUBORD_PACK_RID,
        COALESCE(SUBORD_BC_RID, @NO_RID) AS SUBORD_BC_RID,
        h.HDR_ID AS MASTER_HDR_ID
	FROM MASTER_HEADER mh WITH (NOLOCK), 
		 HEADER h WITH (NOLOCK) 
	WHERE SUBORD_HDR_RID = @SUBORD_HDR_RID
          --AND h.HDR_RID = SUBORD_HDR_RID TT#1750-MD
		  AND h.HDR_RID = MASTER_HDR_RID

END
GO
