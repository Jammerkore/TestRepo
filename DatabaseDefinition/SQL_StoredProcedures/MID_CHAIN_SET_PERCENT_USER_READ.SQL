--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads chain plan set percentages for a user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_SET_PERCENT_USER_READ]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SG_RID, 
		CDR_RID 
	FROM CHAIN_SET_PERCENT_USER 
    WHERE USER_RID = @USER_RID

END
GO
