--dv =============================================
--dv Create date:	11/15/2013
--dv Description:	Updates the style id in the color node table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_NODE_UPDATE_STYLE_ID]
	@NEW_STYLE_NODE_ID varchar(50),
	@OLD_STYLE_NODE_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE COLOR_NODE WITH (ROWLOCK) 
	SET 
		STYLE_NODE_ID = @NEW_STYLE_NODE_ID
    WHERE STYLE_NODE_ID = @OLD_STYLE_NODE_ID

	SELECT @@ROWCOUNT;
END
GO
