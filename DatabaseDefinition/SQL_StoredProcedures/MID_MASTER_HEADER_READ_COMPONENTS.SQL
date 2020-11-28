--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads the components for the master header
--dv Modify:        10/16/2015 TT#1743-Data Layer Request - RMatelic - add nolock to MID_MASTER_HEADER_READ_COMPONENTS to prevent timeout error
--dv                10/20/2015 TT#1745-MD-Data Layer Request - RMatelic - Stored Procedure MID_MASTER_HEADER_READ_COMPONENTS is incorrect
--dv =============================================
CREATE PROCEDURE [dbo].[MID_MASTER_HEADER_READ_COMPONENTS] 
	@MASTER_HDR_RID int
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
        h.HDR_ID AS SUBORD_HDR_ID
	FROM MASTER_HEADER mh WITH (NOLOCK) 
    INNER JOIN HEADER h WITH (NOLOCK) on h.HDR_RID = SUBORD_HDR_RID
	WHERE MASTER_HDR_RID = @MASTER_HDR_RID

END
GO
