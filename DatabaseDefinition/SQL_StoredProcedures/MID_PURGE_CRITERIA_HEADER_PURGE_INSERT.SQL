--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row for purge criteria for headers
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_CRITERIA_HEADER_PURGE_INSERT]
	@HN_RID int,
	@HEADER_TYPE int,
	@PURGE_HEADERS_TIMEFRAME int,
	@PURGE_HEADERS int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PURGE_CRITERIA_HEADER_PURGE (HN_RID, HEADER_TYPE, PURGE_HEADERS_TIMEFRAME, PURGE_HEADERS) 
	VALUES (@HN_RID, @HEADER_TYPE, @PURGE_HEADERS_TIMEFRAME, @PURGE_HEADERS)

	SELECT @@ROWCOUNT;

END
GO
