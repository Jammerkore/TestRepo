--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Updates the header id
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_UPDATE_ID]
	@HDR_RID int,
	@HDR_ID varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE HEADER 
	SET 
		HDR_ID = @HDR_ID 
	WHERE HDR_RID = @HDR_RID

	SELECT @@ROWCOUNT;
END
GO