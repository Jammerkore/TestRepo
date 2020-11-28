--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Inserts a new row into the user option table, filling in theme
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_OPTIONS_INSERT_THEME]
	@USER_RID int,
	@THEME_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO USER_OPTIONS
		(
		USER_RID, 
		THEME_RID
		) 
	VALUES 
		(
		@USER_RID, 
		@THEME_RID
		)

	SELECT @@ROWCOUNT
END
GO
