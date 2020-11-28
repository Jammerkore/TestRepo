--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads rows from the determine node activity info table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DETERMINE_NODE_ACTIVITY_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		PROCESS_RID, 
		COALESCE(TOTAL_NODES,0) TOTAL_NODES,
        COALESCE(ACTIVE_NODES,0) ACTIVE_NODES,
        COALESCE(INACTIVE_NODES,0) INACTIVE_NODES,
        COALESCE(TOTAL_ERRORS,0) TOTAL_ERRORS
    FROM DETERMINE_NODE_ACTIVITY_INFO 
    WHERE PROCESS_RID = @PROCESS_RID

END
GO
