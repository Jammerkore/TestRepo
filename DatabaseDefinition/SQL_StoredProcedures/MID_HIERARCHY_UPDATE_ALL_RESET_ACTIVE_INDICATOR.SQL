--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Sets the active indicator to false for all rows on the hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_UPDATE_ALL_RESET_ACTIVE_INDICATOR]
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE HIERARCHY_NODE 
	SET 
		ACTIVE_IND = '0'

	SELECT @@ROWCOUNT;

END
GO
