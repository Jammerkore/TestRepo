--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Inserts a row into the method change history table
--dv History:		1/20/2014 -TT#1511-MD -jsobek -Data Layer Request - Changes for Method and Workflow change history
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_CHANGE_HISTORY_INSERT]
	@METHOD_RID int,
	@USER_RID int,
	@CHANGE_DATE datetime,
	@METHOD_COMMENT varchar(250),
	@WINDOWS_USER varchar(250),
	@WINDOWS_MACHINE varchar(250),
	@WINDOWS_REMOTE_MACHINE varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_CHANGE_HISTORY
		(
		METHOD_RID, 
		USER_RID, 
		CHANGE_DATE, 
		METHOD_COMMENT,
		WINDOWS_USER,
		WINDOWS_MACHINE,
		WINDOWS_REMOTE_MACHINE
		)
	VALUES 
		(
		@METHOD_RID, 
		@USER_RID, 
		@CHANGE_DATE, 
		@METHOD_COMMENT,
		@WINDOWS_USER,
		@WINDOWS_MACHINE,
		@WINDOWS_REMOTE_MACHINE
		)

	SELECT @@ROWCOUNT;

END
GO