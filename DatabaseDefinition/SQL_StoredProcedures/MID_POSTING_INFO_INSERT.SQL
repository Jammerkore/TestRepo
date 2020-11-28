--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Insert a row into the posting info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_POSTING_INFO_INSERT]
		@PROCESS_RID int, 
		@CH_DAY_HIS_RECS int, 
		@CH_WK_HIS_RECS int, 
		@CH_WK_FOR_RECS int,
		@ST_DAY_HIS_RECS int, 
		@ST_WK_HIS_RECS int, 
		@ST_WK_FOR_RECS int, 
		@INTRANSIT_RECS int, 
		@RECS_WITH_ERRORS int, 
		@NODES_ADDED int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO POSTING_INFO
		(
		PROCESS_RID, 
		CH_DAY_HIS_RECS, 
		CH_WK_HIS_RECS, 
		CH_WK_FOR_RECS,
		ST_DAY_HIS_RECS, 
		ST_WK_HIS_RECS, 
		ST_WK_FOR_RECS, 
		INTRANSIT_RECS, 
		RECS_WITH_ERRORS, 
		NODES_ADDED
		)
	VALUES
		(
		@PROCESS_RID, 
		@CH_DAY_HIS_RECS, 
		@CH_WK_HIS_RECS, 
		@CH_WK_FOR_RECS,
		@ST_DAY_HIS_RECS, 
		@ST_WK_HIS_RECS, 
		@ST_WK_FOR_RECS, 
		@INTRANSIT_RECS, 
		@RECS_WITH_ERRORS, 
		@NODES_ADDED
		)

	SELECT @@ROWCOUNT;

END
GO
