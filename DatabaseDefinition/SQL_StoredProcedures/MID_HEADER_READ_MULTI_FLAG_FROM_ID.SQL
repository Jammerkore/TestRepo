--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns the multi header flag from the header view based on the ID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_MULTI_FLAG_FROM_ID]
	@HDR_ID varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT MultiHeader 
	FROM VW_GET_HEADERS 
	WHERE HDR_ID = @HDR_ID

END
GO
