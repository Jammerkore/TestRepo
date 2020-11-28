--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Updates a infragistics layout
--dv =============================================
CREATE PROCEDURE [dbo].[MID_INFRAGISTICS_LAYOUTS_UPDATE] 
	@USER_RID int,
	@LAYOUT_ID int,
	@LAYOUT_SIZE int, 
	@LAYOUT_CONTENT image
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE INFRAGISTICS_LAYOUTS 
	SET 
		LAYOUT_SIZE = @LAYOUT_SIZE, 
		LAYOUT_CONTENT = @LAYOUT_CONTENT 
	WHERE USER_RID = @USER_RID 
		  AND LAYOUT_ID = @LAYOUT_ID
	
	SELECT @@ROWCOUNT;

END
GO