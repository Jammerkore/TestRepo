--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Updates a row for header bulk color
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_UPDATE]
	@HDR_RID int, 
	@HDR_BC_RID int,
	@COLOR_CODE_RID int,
	@UNITS int,
	@MULTIPLE int,
	@MINIMUM int,
	@MAXIMUM int,
	@RESERVE_UNITS int,
	@SEQ int,
	@NAME varchar(50),
	@DESCRIPTION varchar(100),
	@ASRT_BC_RID int,
	@COLOR_STATUS_FLAGS int, 
	@LAST_BCSZ_KEY_USED int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE HEADER_BULK_COLOR WITH (ROWLOCK) 
    SET 
		COLOR_CODE_RID = @COLOR_CODE_RID, 
		UNITS = @UNITS, 
		MULTIPLE = @MULTIPLE, 
		MINIMUM = @MINIMUM, 
		MAXIMUM = @MAXIMUM, 
		RESERVE_UNITS = @RESERVE_UNITS, 
		SEQ = @SEQ,
        NAME = @NAME, 
		DESCRIPTION = @DESCRIPTION, 
		ASRT_BC_RID = @ASRT_BC_RID, 
		LAST_BCSZ_KEY_USED = @LAST_BCSZ_KEY_USED, 
		COLOR_STATUS_FLAGS = @COLOR_STATUS_FLAGS  
    WHERE HDR_RID = @HDR_RID 
		  AND HDR_BC_RID = @HDR_BC_RID

	SELECT @@ROWCOUNT;

END
GO
