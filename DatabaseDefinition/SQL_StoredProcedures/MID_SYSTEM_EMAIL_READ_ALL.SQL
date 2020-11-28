--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Reads all system email rows
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_EMAIL_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM SYSTEM_EMAIL

END
GO