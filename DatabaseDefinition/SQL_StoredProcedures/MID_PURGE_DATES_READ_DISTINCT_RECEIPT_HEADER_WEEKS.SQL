--dv =============================================
--dv Create date: 02/04/2014
--dv Description: Get distinct purge dates by receipt header weeks.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_READ_DISTINCT_RECEIPT_HEADER_WEEKS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT PURGE_HEADERS_WEEKS_RECEIPT
	FROM PURGE_DATES WITH (NOLOCK)
END
GO