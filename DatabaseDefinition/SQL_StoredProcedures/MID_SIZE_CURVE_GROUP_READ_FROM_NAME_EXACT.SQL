--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Reads a size curve group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT]
	@SIZE_CURVE_GROUP_NAME varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM SIZE_CURVE_GROUP 
	WHERE SIZE_CURVE_GROUP_NAME = @SIZE_CURVE_GROUP_NAME

END
GO
