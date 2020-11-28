--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Deletes all header release types
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_TYPE_RELEASE_DELETE_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM HEADER_TYPE_RELEASE
		
	SELECT @@ROWCOUNT;

END
GO