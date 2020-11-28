--dv =============================================
--dv Create date: 12/12/2013
--dv Description:	Deletes the user's current grid view.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_CURRENT_GRID_VIEW_DELETE]
	@USER_RID INT,
	@LAYOUT_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM USER_CURRENT_GRID_VIEW
	WHERE USER_RID = @USER_RID
		AND LAYOUT_ID = @LAYOUT_ID

	SELECT @@ROWCOUNT
END
GO




















