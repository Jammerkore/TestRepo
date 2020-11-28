--dv =============================================
--dv Create date:	10/30/2013
--dv Description:	Reads data for column defaults from the user allocation table based on user rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ALLOCATION_READ_CURRENT_COLUMNS]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		VIEW_TYPE, 
		GROUP_BY_ID
	FROM USER_ALLOCATION 
	WHERE USER_RID = @USER_RID

END
GO
