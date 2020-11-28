--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Reads infragistics layouts for the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_INFRAGISTICS_LAYOUTS_READ_FROM_USER] 
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM INFRAGISTICS_LAYOUTS 
	WHERE USER_RID = @USER_RID

END
GO