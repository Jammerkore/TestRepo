--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Returns the display string for the hierarchy node rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_READ_NODE_DISPLAY] 
	@NODE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT dbo.UDF_MID_GET_NODE_DISPLAY (@NODE_RID)

END
GO
