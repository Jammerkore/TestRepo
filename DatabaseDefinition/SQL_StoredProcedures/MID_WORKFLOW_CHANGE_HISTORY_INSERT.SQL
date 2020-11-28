--dv =============================================
--dv Create date: 12/06/2013
--dv Description: Inserts workflow change history records.
--dv History:		1/20/2014 -TT#1511-MD -jsobek -Data Layer Request - Changes for Method and Workflow change history
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_CHANGE_HISTORY_INSERT]
	@WORKFLOW_RID INT,
	@USER_RID INT,
	@CHANGE_DATE DATETIME,
	@WORKFLOW_COMMENT VARCHAR(250),
	@WINDOWS_USER varchar(250),
	@WINDOWS_MACHINE varchar(250),
	@WINDOWS_REMOTE_MACHINE varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO WORKFLOW_CHANGE_HISTORY (
		WORKFLOW_RID,
		USER_RID,
		CHANGE_DATE,
		WORKFLOW_COMMENT,
		WINDOWS_USER,
		WINDOWS_MACHINE,
		WINDOWS_REMOTE_MACHINE
		)
	VALUES (
		@WORKFLOW_RID,
		@USER_RID,
		@CHANGE_DATE,
		@WORKFLOW_COMMENT,
		@WINDOWS_USER,
		@WINDOWS_MACHINE,
		@WINDOWS_REMOTE_MACHINE
		)

	SELECT @@ROWCOUNT
END
GO




