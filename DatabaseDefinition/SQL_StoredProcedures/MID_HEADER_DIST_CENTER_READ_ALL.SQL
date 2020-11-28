--dv =============================================
--dv Create date:	4/21/2017
--dv Description:	Gets all distribution center names from the HEADER table
--dv History:		TT#1966 - MD - JSmith - DC Fulfillment
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_DIST_CENTER_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT DIST_CENTER
	FROM HEADER
	WHERE DIST_CENTER > ''
	ORDER BY DIST_CENTER
END
GO
