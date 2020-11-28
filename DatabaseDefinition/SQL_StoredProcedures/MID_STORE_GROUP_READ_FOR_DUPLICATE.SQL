--dv =============================================
--dv Create date:	3/26/2015
--dv Description:	Returns a count of rows that match the group name and user
--dv				Case Insensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_READ_FOR_DUPLICATE]
	@SG_ID VARCHAR(50),
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*) AS MYCOUNT
	FROM STORE_GROUP
	WHERE UPPER(SG_ID)=UPPER(@SG_ID)
		AND USER_RID = @USER_RID
		AND IS_ACTIVE=1

END
GO