--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the hierarchy reclass info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_RECLASS_INFO_INSERT]
	@PROCESS_RID int,
	@HEIR_TRANS_WRITTEN int,
	@ADDCHG_TRANS_WRITTEN int,
	@DELETE_TRANS_WRITTEN int,
	@MOVE_TRANS_WRITTEN int,
	@TRANS_REJECTED int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO HIER_RECLASS_INFO 
		(
		PROCESS_RID,
		HEIR_TRANS_WRITTEN, 
		ADDCHG_TRANS_WRITTEN,
        DELETE_TRANS_WRITTEN, 
		MOVE_TRANS_WRITTEN, 
		TRANS_REJECTED
		) 
	VALUES
		(
		@PROCESS_RID,
		@HEIR_TRANS_WRITTEN, 
		@ADDCHG_TRANS_WRITTEN,
        @DELETE_TRANS_WRITTEN, 
		@MOVE_TRANS_WRITTEN, 
		@TRANS_REJECTED
		)
		
	SELECT @@ROWCOUNT;
		
END
GO
