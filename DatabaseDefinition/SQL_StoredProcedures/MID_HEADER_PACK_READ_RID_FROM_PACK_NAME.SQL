--dv =============================================
--dv Create date: 02/05/2014
--dv Description: Reads the pack rid from the pack name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_READ_RID_FROM_PACK_NAME]
	@HDR_RID INT,
	@HDR_PACK_NAME VARCHAR(100)
AS
BEGIN
	DECLARE @HDR_PACK_RID int
	SET @HDR_PACK_RID = -1
	SET NOCOUNT ON;

	SELECT 
		@HDR_PACK_RID = HDR_PACK_RID
	FROM HEADER_PACK 
	WHERE HDR_RID = @HDR_RID
		  AND HDR_PACK_NAME = @HDR_PACK_NAME

	SELECT @HDR_PACK_RID

END
GO


