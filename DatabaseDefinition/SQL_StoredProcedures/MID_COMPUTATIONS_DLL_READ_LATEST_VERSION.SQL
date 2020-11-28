--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads the latest version in the computations dll table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATIONS_DLL_READ_LATEST_VERSION]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		VERSION 
	FROM COMPUTATIONS_DLL 
	ORDER BY CREATION_DATE DESC

END
GO

