--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Inserts a new row into the user option table, filling in the my hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_OPTIONS_INSERT_MY_HIERARCHY]
	@USER_RID int,
	@MY_HIERARCHY varchar(250),
	@MY_HIERARCHY_COLOR varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO USER_OPTIONS
		(
		USER_RID, 
		MY_HIERARCHY, 
		MY_HIERARCHY_COLOR
		)
	VALUES 
		(
		@USER_RID, 
		@MY_HIERARCHY, 
		@MY_HIERARCHY_COLOR
		)

	SELECT @@ROWCOUNT
END
GO
