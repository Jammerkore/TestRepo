--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Creates the index for id on the color and size node tables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_CREATE_COLOR_AND_SIZE_ID_INDEX]
AS
BEGIN
	SET NOCOUNT ON;

	CREATE INDEX STYLE_NODE_ID_IDX ON COLOR_NODE (STYLE_NODE_ID)  
	CREATE INDEX STYLE_COLOR_NODE_ID_IDX ON SIZE_NODE (	STYLE_NODE_ID,	COLOR_NODE_ID)

END
GO
