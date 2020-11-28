--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Insert a row into the determine node activity info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DETERMINE_NODE_ACTIVITY_INFO_INSERT]
	@PROCESS_RID int,
	@TOTAL_NODES int, 
	@ACTIVE_NODES int,
    @INACTIVE_NODES int, 
	@TOTAL_ERRORS int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO DETERMINE_NODE_ACTIVITY_INFO
		(
		PROCESS_RID, 
		TOTAL_NODES, 
		ACTIVE_NODES,
        INACTIVE_NODES, 
		TOTAL_ERRORS
		) 
	VALUES
		(
		@PROCESS_RID, 
		@TOTAL_NODES, 
		@ACTIVE_NODES,
        @INACTIVE_NODES, 
		@TOTAL_ERRORS
		)

	SELECT @@ROWCOUNT;

END
GO
