--dv =============================================
--dv Create date:	6/26/2014
--dv Description:	Reads the default filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_READ_DEFAULT]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 * 
	FROM FILTER 
	--WHERE STORE_FILTER_RID = -1

END
GO


