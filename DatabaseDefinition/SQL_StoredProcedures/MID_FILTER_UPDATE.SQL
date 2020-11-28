--dv =============================================
--dv Create date:	6/25/2014
--dv Description:	Updates a filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_UPDATE]
	@FILTER_RID INT,
	@USER_RID INT,
	@OWNER_USER_RID INT,  -- TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
	@FILTER_NAME VARCHAR(250),
	@IS_LIMITED BIT,
	@RESULT_LIMIT INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE FILTER
    SET 
		USER_RID = @USER_RID,
		OWNER_USER_RID = @OWNER_USER_RID,  -- TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
		FILTER_NAME = @FILTER_NAME, 
		IS_LIMITED = @IS_LIMITED, 
		RESULT_LIMIT = @RESULT_LIMIT
    WHERE FILTER_RID = @FILTER_RID

	SELECT @@ROWCOUNT;

END

GO