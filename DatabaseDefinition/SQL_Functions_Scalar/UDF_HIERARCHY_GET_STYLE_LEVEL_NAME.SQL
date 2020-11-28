--dv Create date:	9/12/2013
--dv Description:	Returns the style level name from product hierarchy levels
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_STYLE_LEVEL_NAME]
(
)
RETURNS varchar(50)
AS
BEGIN
	DECLARE @STYLE_LEVEL_NAME varchar(50)

	SELECT @STYLE_LEVEL_NAME= PHL_ID
	FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] with (nolock)
	WHERE PHL_TYPE=800202

	RETURN @STYLE_LEVEL_NAME

END
GO
