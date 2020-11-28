--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Updates the my hierarchy in the user option table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_OPTIONS_UPDATE_MY_HIERARCHY]
	@USER_RID int,
	@MY_HIERARCHY varchar(250),
	@MY_HIERARCHY_COLOR varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_OPTIONS 
		SET 
		MY_HIERARCHY = @MY_HIERARCHY,
		MY_HIERARCHY_COLOR = @MY_HIERARCHY_COLOR
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT
END
GO
