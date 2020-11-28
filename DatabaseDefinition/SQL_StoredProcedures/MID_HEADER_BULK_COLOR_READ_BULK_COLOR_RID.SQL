--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads the bulk color rid from the header and color code rids
--dv Modified date : 8/17/2015
--dv History: TT#1704-MD - RMatelic - Data Layer Request - Update procedure to read bulk colors       
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_READ_BULK_COLOR_RID] 
	@HDR_RID INT,
	@COLOR_CODE_RID INT
AS
BEGIN
    DECLARE @HDR_COLOR_RID int
	SET @HDR_COLOR_RID = -1
	SET NOCOUNT ON;
	
	SELECT 
		@HDR_COLOR_RID = HDR_BC_RID 
	FROM HEADER_BULK_COLOR 
	WHERE HDR_RID = @HDR_RID
		  AND COLOR_CODE_RID = @COLOR_CODE_RID

	SELECT @HDR_COLOR_RID
END
GO