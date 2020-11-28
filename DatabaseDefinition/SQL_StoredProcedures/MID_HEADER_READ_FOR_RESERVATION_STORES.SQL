--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets headers with reservation stores (IMO)
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_FOR_RESERVATION_STORES]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM VW_GET_HEADERS 
	WHERE IMO_ID IS NOT NULL
		  AND IMO_ID <> ''

END
GO
