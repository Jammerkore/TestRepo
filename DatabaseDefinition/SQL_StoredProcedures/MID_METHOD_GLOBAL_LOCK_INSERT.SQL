--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Inserts a global lock method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_LOCK_INSERT] 
	@METHOD_RID int,
	@HN_RID int, 
	@FV_RID int, 
	@CDR_RID int,
	@MULTI_LEVEL_IND char(1), 
	@FROM_LEVEL_TYPE int, 
	@FROM_LEVEL_SEQ int, 
	@FROM_LEVEL_OFFSET int, 
	@TO_LEVEL_TYPE int,
	@TO_LEVEL_SEQ int, 
	@TO_LEVEL_OFFSET int, 
	@STORE_IND char(1), 
	@CHAIN_IND char(1), 
	@STORE_FILTER_RID int, 
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_GLOBAL_LOCK
		(
		METHOD_RID, 
		HN_RID, 
		FV_RID, 
		CDR_RID,
		MULTI_LEVEL_IND, 
		FROM_LEVEL_TYPE, 
		FROM_LEVEL_SEQ, 
		FROM_LEVEL_OFFSET, 
		TO_LEVEL_TYPE,
		TO_LEVEL_SEQ, 
		TO_LEVEL_OFFSET, 
		STORE_IND, 
		CHAIN_IND, 
		STORE_FILTER_RID, 
		OLL_RID
		)
    VALUES
		(
		@METHOD_RID, 
		@HN_RID, 
		@FV_RID, 
		@CDR_RID,
		@MULTI_LEVEL_IND, 
		@FROM_LEVEL_TYPE, 
		@FROM_LEVEL_SEQ, 
		@FROM_LEVEL_OFFSET, 
		@TO_LEVEL_TYPE,
		@TO_LEVEL_SEQ, 
		@TO_LEVEL_OFFSET, 
		@STORE_IND, 
		@CHAIN_IND, 
		@STORE_FILTER_RID, 
		@OLL_RID
		)
	 
	 SELECT @@ROWCOUNT;

END
GO













