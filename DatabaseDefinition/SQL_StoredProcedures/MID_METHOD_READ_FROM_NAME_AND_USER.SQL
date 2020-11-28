--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads methods based on the name and user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_FROM_NAME_AND_USER] 
	@METHOD_NAME varchar(50),
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM METHOD 
	WHERE METHOD_NAME = @METHOD_NAME
		  AND USER_RID = @USER_RID

END
GO