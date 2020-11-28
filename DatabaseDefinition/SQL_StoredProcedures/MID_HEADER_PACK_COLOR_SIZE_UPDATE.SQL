--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Updates a row for header pack color size
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_COLOR_SIZE_UPDATE] 
	@HDR_PC_RID int,
	@HDR_PCSZ_KEY int,
	@SIZE_CODE_RID int,
	@UNITS int,
	@SEQ int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE HEADER_PACK_COLOR_SIZE
    SET
		SIZE_CODE_RID = @SIZE_CODE_RID, 
		UNITS = @UNITS, 
		SEQ = @SEQ
    WHERE HDR_PC_RID = @HDR_PC_RID 
	      AND HDR_PCSZ_KEY = @HDR_PCSZ_KEY
	
	SELECT @@ROWCOUNT;

END
GO
