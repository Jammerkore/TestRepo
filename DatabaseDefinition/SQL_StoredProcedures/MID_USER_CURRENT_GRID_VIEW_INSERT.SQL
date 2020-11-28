--dv =============================================
--dv Create date: 12/12/2013
--dv Description:	Inserts user's current grid view settings.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_CURRENT_GRID_VIEW_INSERT]
	@USER_RID INT,
	@LAYOUT_ID INT,
	@VIEW_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO USER_CURRENT_GRID_VIEW 
		(
		USER_RID,
		LAYOUT_ID,
		VIEW_RID
		)
	VALUES 
		(
		@USER_RID,
		@LAYOUT_ID,
		@VIEW_RID
		)

	SELECT @@ROWCOUNT
END
GO




















