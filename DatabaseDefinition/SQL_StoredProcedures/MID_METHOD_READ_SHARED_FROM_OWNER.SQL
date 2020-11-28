--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads shared methods based on the method type id, user, and owner
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_SHARED_FROM_OWNER] 
	@USER_RID int,
	@VIRTUAL_IND char(1),
	@OWNER_USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM VW_METHOD_READ
	WHERE USER_RID = @USER_RID
		  AND VIRTUAL_IND = @VIRTUAL_IND
		  AND USER_RID <> OWNER_USER_RID
		  AND OWNER_USER_RID = @OWNER_USER_RID

END
GO