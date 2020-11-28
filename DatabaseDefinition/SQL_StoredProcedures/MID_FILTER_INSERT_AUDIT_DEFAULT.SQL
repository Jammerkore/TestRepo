--dv =============================================
--dv Create date:	12/30/2014
--dv Description:	Inserts the default audit filter for a user and returns the new filter RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_INSERT_AUDIT_DEFAULT]
	@USER_RID INT,
	@FILTER_RID INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO FILTER
	(
		USER_RID,
		OWNER_USER_RID,
		FILTER_TYPE,
		FILTER_NAME,
		IS_LIMITED,
		RESULT_LIMIT
	) 
	VALUES
	(
		@USER_RID,
		@USER_RID,
		5,
		'Default Audit Filter',
		0,
		0
	)

	SELECT @FILTER_RID = SCOPE_IDENTITY()

END
GO
