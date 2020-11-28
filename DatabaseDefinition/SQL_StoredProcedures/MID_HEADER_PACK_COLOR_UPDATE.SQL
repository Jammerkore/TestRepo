--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Updates a header pack color row
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_COLOR_UPDATE]
	@HDR_PC_RID int,
	@UNITS int,
	@SEQ int,
	@NAME varchar(50),
	@DESCRIPTION varchar(100),
	@LAST_PCSZ_KEY_USED int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE HEADER_PACK_COLOR
    SET 
		UNITS = @UNITS, 
		SEQ = @SEQ, 
		NAME = @NAME, 
		DESCRIPTION = @DESCRIPTION, 
		LAST_PCSZ_KEY_USED = @LAST_PCSZ_KEY_USED
    WHERE HDR_PC_RID = @HDR_PC_RID

	SELECT @@ROWCOUNT;

END
GO
