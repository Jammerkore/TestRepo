--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Deletes a infragistics layout
--dv =============================================
CREATE PROCEDURE [dbo].[MID_INFRAGISTICS_LAYOUTS_DELETE_FROM_LAYOUT] 
	@LAYOUT_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM INFRAGISTICS_LAYOUTS 
	WHERE LAYOUT_ID = @LAYOUT_ID
	
	SELECT @@ROWCOUNT;

END
GO