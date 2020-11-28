--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a tasklist
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASKLIST_DELETE]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM TASKLIST 
	WHERE TASKLIST_RID =@TASKLIST_RID

	SELECT @@ROWCOUNT;
END
GO