--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Deletes a infragistics layout
--dv =============================================
CREATE PROCEDURE [dbo].[MID_INFRAGISTICS_LAYOUTS_DELETE] 
	@USER_RID int,
	@LAYOUT_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM INFRAGISTICS_LAYOUTS 
	WHERE USER_RID = @USER_RID AND LAYOUT_ID = @LAYOUT_ID
	
	SELECT @@ROWCOUNT;

END
GO