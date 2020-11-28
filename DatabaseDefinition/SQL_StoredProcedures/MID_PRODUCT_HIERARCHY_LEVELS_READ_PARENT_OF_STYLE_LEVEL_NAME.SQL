--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Reads parent of style level name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_LEVELS_READ_PARENT_OF_STYLE_LEVEL_NAME]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[UDF_HIERARCHY_GET_PARENT_OF_STYLE_LEVEL_NAME] () AS PARENT_OF_STYLE_LEVEL_NAME

END
GO