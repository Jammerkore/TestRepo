--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns all non-released headers
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_NON_RELEASED]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HEADER_RELEASED_STATUS int = 802709;

	SELECT * 
	FROM HEADER WITH (NOLOCK)  
	WHERE DISPLAY_STATUS <> @HEADER_RELEASED_STATUS

END
GO
