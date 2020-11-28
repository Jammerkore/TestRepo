--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Drops the index for id on the color and size node tables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_DROP_COLOR_AND_SIZE_ID_INDEX]
AS
BEGIN
	SET NOCOUNT ON;

	DROP INDEX COLOR_NODE.STYLE_NODE_ID_IDX 
	DROP INDEX SIZE_NODE.STYLE_COLOR_NODE_ID_IDX

END
GO
