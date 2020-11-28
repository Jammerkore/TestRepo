--dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Updates Forecast Model table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_UNLOCK_UPDATE]
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

	UPDATE METHOD_GLOBAL_UNLOCK 
	SET 
		HN_RID = @HN_RID,
		FV_RID = @FV_RID,
		CDR_RID = @CDR_RID,
        MULTI_LEVEL_IND = @MULTI_LEVEL_IND,
        FROM_LEVEL_TYPE = @FROM_LEVEL_TYPE,
        FROM_LEVEL_SEQ = @FROM_LEVEL_SEQ,
        FROM_LEVEL_OFFSET = @FROM_LEVEL_OFFSET,
        TO_LEVEL_TYPE = @TO_LEVEL_TYPE,
        TO_LEVEL_SEQ = @TO_LEVEL_SEQ,
		TO_LEVEL_OFFSET = @TO_LEVEL_OFFSET,
        STORE_IND = @STORE_IND,
        CHAIN_IND = @CHAIN_IND,
        STORE_FILTER_RID = @STORE_FILTER_RID,
		OLL_RID = @OLL_RID 
	WHERE
        METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT
END
GO