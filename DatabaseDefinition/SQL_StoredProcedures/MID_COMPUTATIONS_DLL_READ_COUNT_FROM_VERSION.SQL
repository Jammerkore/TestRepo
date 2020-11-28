--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Counts rows in the computations dll table based on version
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION]
	@VERSION varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) MyCount 
	FROM COMPUTATIONS_DLL 
	WHERE VERSION = @VERSION

END
GO
