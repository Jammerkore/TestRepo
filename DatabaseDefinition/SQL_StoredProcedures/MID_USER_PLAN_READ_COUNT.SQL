--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads the count of the user plans for the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_READ_COUNT] 
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		COUNT(*) MyCount 
	FROM USER_PLAN WITH (NOLOCK)
    WHERE USER_RID = @USER_RID

END
GO