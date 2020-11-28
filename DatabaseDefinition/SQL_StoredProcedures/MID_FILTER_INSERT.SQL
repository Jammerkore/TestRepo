--dv =============================================
--dv Create date:	6/25/2014
--dv Description:	Inserts a filter and returns the new filter RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_INSERT]
	@USER_RID INT,
	@OWNER_USER_RID INT,
	@FILTER_TYPE INT,
	@FILTER_NAME VARCHAR(250),
	@IS_LIMITED BIT,
	@RESULT_LIMIT INT,
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
		@OWNER_USER_RID,
		@FILTER_TYPE,
		@FILTER_NAME,
		@IS_LIMITED,
		@RESULT_LIMIT
	)

	SELECT @FILTER_RID = SCOPE_IDENTITY()

END
GO
