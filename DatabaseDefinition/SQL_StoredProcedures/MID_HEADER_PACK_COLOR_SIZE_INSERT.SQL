--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Inserts a row for header pack color size
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_COLOR_SIZE_INSERT]
	@HDR_PACK_RID int, 
	@HDR_PC_RID int,
	@HDR_PCSZ_KEY int,
	@SIZE_CODE_RID int,
	@UNITS int,
	@SEQ int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO HEADER_PACK_COLOR_SIZE (HDR_PACK_RID, HDR_PC_RID, HDR_PCSZ_KEY, SIZE_CODE_RID, UNITS, SEQ )
    VALUES(@HDR_PACK_RID, @HDR_PC_RID, @HDR_PCSZ_KEY, @SIZE_CODE_RID, @UNITS, @SEQ)

	SELECT @@ROWCOUNT;

END
GO
