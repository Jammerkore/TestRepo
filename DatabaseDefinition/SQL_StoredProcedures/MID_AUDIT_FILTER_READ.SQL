--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Reads rows form the audit filter table based on the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_FILTER_READ]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM AUDIT_FILTER
	WHERE USER_RID = @USER_RID

END
GO