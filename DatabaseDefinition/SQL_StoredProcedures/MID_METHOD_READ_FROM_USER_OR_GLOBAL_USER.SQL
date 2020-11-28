--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads methods based on the user or global user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER] 
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GlobalUserRID int = 4;

	SELECT * 
	FROM VW_METHOD_READ
	WHERE ( USER_RID = @GlobalUserRID
		    OR USER_RID = @USER_RID
		  )

END
GO