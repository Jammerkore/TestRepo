--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads the latest dll image in the computations dll table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		DLL_IMAGE 
	FROM COMPUTATIONS_DLL 
	ORDER BY CREATION_DATE DESC

END
GO

