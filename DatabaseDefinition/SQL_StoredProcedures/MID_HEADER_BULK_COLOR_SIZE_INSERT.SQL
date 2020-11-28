--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Inserts a row for header bulk color size
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_SIZE_INSERT]
	@HDR_RID int, 
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
	
	INSERT INTO HEADER_BULK_COLOR_SIZE
		(
		HDR_RID, 
		HDR_BC_RID, 
		HDR_BCSZ_KEY, 
		SIZE_CODE_RID, 
		UNITS,
		MULTIPLE, 
		MINIMUM, 
		MAXIMUM, 
		RESERVE_UNITS, 
		SEQ
		)
	VALUES 
		(
		@HDR_RID, 
		@HDR_BC_RID, 
		@HDR_BCSZ_KEY, 
		@SIZE_CODE_RID, 
		@UNITS, 
		@MULTIPLE, 
		@MINIMUM, 
		@MAXIMUM, 
		@RESERVE_UNITS, 
		@SEQ
		)
	
	SELECT @@ROWCOUNT;

END
GO
