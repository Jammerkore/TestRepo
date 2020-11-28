--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Inserts a infragistics layout
--dv =============================================
CREATE PROCEDURE [dbo].[MID_INFRAGISTICS_LAYOUTS_INSERT] 
	@USER_RID int,
	@LAYOUT_ID int,
	@LAYOUT_SIZE int, 
	@LAYOUT_CONTENT image
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO INFRAGISTICS_LAYOUTS
		(
		USER_RID, 
		LAYOUT_ID, 
		LAYOUT_SIZE, 
		LAYOUT_CONTENT
		) 
	VALUES
		(
		@USER_RID, 
		@LAYOUT_ID, 
		@LAYOUT_SIZE, 
		@LAYOUT_CONTENT
		)
	
	SELECT @@ROWCOUNT;

END
GO