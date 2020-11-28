--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Counts rows in the computations dll table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATIONS_DLL_READ_COUNT]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	COUNT(*) MyCount 
	FROM COMPUTATIONS_DLL

END
GO


