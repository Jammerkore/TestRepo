--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Reads the style level name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_LEVELS_READ_STYLE_LEVEL_NAME] 
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [dbo].[UDF_HIERARCHY_GET_STYLE_LEVEL_NAME] () AS STYLE_LEVEL_NAME

END
GO