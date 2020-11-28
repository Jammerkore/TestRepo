--dv =============================================
--dv Create date: 2/9/2018
--dv Description:	Write Extract Session Data
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Extract_Session_Write]
	@SessionKey BIGINT,
	@SessionActiveInd CHAR,
	@SessionStartDateTime DATETIME,
	@SessionEndDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON
	IF EXISTS (SELECT * FROM Extract_Session WHERE SessionKey = @SessionKey) 
	BEGIN
	   UPDATE Extract_Session with (ROWLOCK) 
		  SET SessionActiveInd = @SessionActiveInd,
			  SessionStartDateTime = @SessionStartDateTime,
			  SessionEndDateTime = @SessionEndDateTime		
		WHERE SessionKey = @SessionKey 
	END
	ELSE
	BEGIN
		INSERT INTO Extract_Session (
		SessionKey, 
		SessionActiveInd, 
		SessionStartDateTime, 
		SessionEndDateTime
		)
		VALUES (
		@SessionKey, 
		@SessionActiveInd, 
		@SessionStartDateTime,
		@SessionEndDateTime
		)
	END
END
GO






















