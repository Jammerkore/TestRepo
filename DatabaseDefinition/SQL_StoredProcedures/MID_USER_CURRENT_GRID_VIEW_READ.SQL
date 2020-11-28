--dv =============================================
--dv Create date: 12/12/2013
--dv Description:	Get the user's current grid view settings.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_CURRENT_GRID_VIEW_READ]
	@USER_RID INT,
	@LAYOUT_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM USER_CURRENT_GRID_VIEW
	WHERE USER_RID = @USER_RID
		AND LAYOUT_ID = @LAYOUT_ID

END
GO




















