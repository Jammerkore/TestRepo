--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads the method based on the method type id and user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_FROM_TYPE_AND_USER] 
	@METHOD_TYPE_ID int,
	@USER_RID int,
	@VIRTUAL_IND char(1)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM VW_METHOD_READ
	WHERE METHOD_TYPE_ID = @METHOD_TYPE_ID
		  AND USER_RID = @USER_RID
		  AND VIRTUAL_IND = @VIRTUAL_IND

END
GO