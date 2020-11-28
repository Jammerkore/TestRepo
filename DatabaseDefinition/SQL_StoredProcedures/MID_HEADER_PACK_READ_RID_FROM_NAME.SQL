--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Gets pack rid based on the header rid and pack name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_READ_RID_FROM_NAME]
	@HDR_RID int,
	@HDR_PACK_NAME varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT HDR_PACK_RID 
	FROM HEADER_PACK 
	WHERE HDR_RID = @HDR_RID 
		  AND HDR_PACK_NAME = @HDR_PACK_NAME

	SET TRANSACTION ISOLATION LEVEL READ COMMITTED

END
GO
