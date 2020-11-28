--dv =============================================
--dv Create date:	3/24/2015
--dv Description:	Returns a count of rows that match the proposed characteristic group name
--dv				Case Insensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_GROUP_READ_NAME_FOR_DUPLICATE]
	@PROPOSED_GROUP_NAME VARCHAR(50),
	@SCG_EDIT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	
	SELECT COUNT(*) AS MYCOUNT
	FROM STORE_CHAR_GROUP
	WHERE UPPER(SCG_ID)=UPPER(@PROPOSED_GROUP_NAME)
		AND SCG_RID <> @SCG_EDIT_RID



END
GO