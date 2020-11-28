--dv =============================================
--dv Create date: 2/20/2018
--dv Description:	Close all active extract sessions
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Extract_Session_Update_All_For_Close]
	@SessionEndDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON
	UPDATE Extract_Session with (ROWLOCK) 
	   SET SessionActiveInd = '0',
	       SessionEndDateTime = @SessionEndDateTime
	 WHERE SessionActiveInd != '0'
END
GO






















