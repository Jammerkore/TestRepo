--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Updates a row for header bulk color size
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_SIZE_UPDATE]
	@HDR_BC_RID int, 
	@HDR_BCSZ_KEY int,
	@SIZE_CODE_RID int,
	@UNITS int,
	@MULTIPLE int,
	@MINIMUM int,
	@MAXIMUM int,
	@RESERVE_UNITS int,
	@SEQ int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE HEADER_BULK_COLOR_SIZE
    SET 
		SIZE_CODE_RID = @SIZE_CODE_RID, 
		UNITS = @UNITS, 
		MULTIPLE = @MULTIPLE, 
		MINIMUM = @MINIMUM, 
		MAXIMUM = @MAXIMUM, 
		RESERVE_UNITS = @RESERVE_UNITS, 
		SEQ = @SEQ 
    WHERE HDR_BC_RID = @HDR_BC_RID 
		  AND HDR_BCSZ_KEY = @HDR_BCSZ_KEY
	
	SELECT @@ROWCOUNT;

END
GO
