--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Reads all application versions
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_VERSION_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM APPLICATION_VERSION 
	ORDER BY MAJOR_VERSION,MINOR_VERSION,REVISION,MODIFICATION

END
GO