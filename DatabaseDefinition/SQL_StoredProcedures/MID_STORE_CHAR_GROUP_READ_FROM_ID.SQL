--dv =============================================
--dv Create date: 3/26/2015
--dv Description:	Returns the characteristic group from the group id provided.
--dv				Case Insensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_GROUP_READ_FROM_ID]
	@SCG_ID VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM STORE_CHAR_GROUP
	WHERE UPPER(SCG_ID)=UPPER(@SCG_ID)
		

END
GO