--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Updates a header pack row
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_UPDATE]
	@HDR_PACK_RID int,
	@HDR_RID int,
	@HDR_PACK_NAME varchar(100),
	@PACKS int,
	@MULTIPLE int,
	@RESERVE_PACKS int,
	@GENERIC_IND char,
	@ASSOCIATED_PACK_RID int,
	@SEQ int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE HEADER_PACK 
	SET 
        HDR_RID = @HDR_RID, 
		HDR_PACK_NAME = @HDR_PACK_NAME, 
		PACKS = @PACKS, 
		MULTIPLE = @MULTIPLE, 
		RESERVE_PACKS = @RESERVE_PACKS, 
		GENERIC_IND = @GENERIC_IND,
        ASSOCIATED_PACK_RID = @ASSOCIATED_PACK_RID,
        SEQ = @SEQ 
	WHERE HDR_PACK_RID = @HDR_PACK_RID

	SELECT @@ROWCOUNT;

END
GO
