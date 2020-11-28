--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Inserts a color min max for the overrride method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_COLOR_MINMAX_INSERT]
	@METHOD_RID int, 
	@COLOR_CODE_RID int, 
	@COLOR_MIN int, 
	@COLOR_MAX int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_OVERRIDE_COLOR_MINMAX 
		(
		METHOD_RID, 
		COLOR_CODE_RID, 
		COLOR_MIN, 
		COLOR_MAX
		)
	VALUES
		(
		@METHOD_RID, 
		@COLOR_CODE_RID, 
		@COLOR_MIN, 
		@COLOR_MAX
		)

	SELECT @@ROWCOUNT;
END
GO