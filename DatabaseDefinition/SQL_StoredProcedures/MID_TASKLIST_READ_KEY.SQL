--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads a tasklist key
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASKLIST_READ_KEY]
	@TASKLIST_NAME varchar(50), 
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASKLIST 
	WHERE TASKLIST_NAME = @TASKLIST_NAME 
		  AND USER_RID = @USER_RID

END
GO