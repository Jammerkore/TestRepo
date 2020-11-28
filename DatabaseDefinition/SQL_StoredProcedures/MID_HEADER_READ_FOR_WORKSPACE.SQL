--dv =============================================
--dv Create date:	5/29/2015
--dv Description:	Returns the header based on the RID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_FOR_WORKSPACE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HDR_RIDS AS HDR_RID_TYPE;
	
	INSERT INTO @HDR_RIDS
	 select @HDR_RID

	SELECT * FROM (SELECT * FROM [dbo].[UDF_HEADER_READ_FOR_WORKSPACE] (@HDR_RIDS) ) AS h
		ORDER BY h.HDR_ID

END
GO
