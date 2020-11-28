--dv Create date:	9/12/2013
--dv Description:	Returns the parent of style level name from product hierarchy levels
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_PARENT_OF_STYLE_LEVEL_NAME]
(
)
RETURNS varchar(50)
AS
BEGIN
	DECLARE @PARENT_OF_STYLE_LEVEL_NAME varchar(50)

	DECLARE @STYLE_SEQUENCE INT

	--get the style sequence and subtract one to get the parent of style
	SELECT @STYLE_SEQUENCE= PHL_SEQUENCE
	FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] with (nolock)
	WHERE PHL_TYPE=800202

	DECLARE @PARENT_OF_STYLE_SEQUENCE INT
	SET @PARENT_OF_STYLE_SEQUENCE = @STYLE_SEQUENCE - 1;

	SELECT @PARENT_OF_STYLE_LEVEL_NAME= PHL_ID
	FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] with (nolock)
	WHERE PHL_SEQUENCE=@PARENT_OF_STYLE_SEQUENCE

	RETURN @PARENT_OF_STYLE_LEVEL_NAME

END
GO
