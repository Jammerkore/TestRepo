--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Deletes infragistics layouts for the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_USER] 
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM INFRAGISTICS_LAYOUTS 
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO