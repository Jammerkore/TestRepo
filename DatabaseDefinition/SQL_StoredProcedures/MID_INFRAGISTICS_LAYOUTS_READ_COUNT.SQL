--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Reads the count of infragistics layouts for the given user and layout
--dv =============================================
CREATE PROCEDURE [dbo].[MID_INFRAGISTICS_LAYOUTS_READ_COUNT] 
	@USER_RID int,
	@LAYOUT_ID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM INFRAGISTICS_LAYOUTS 
	WHERE USER_RID = @USER_RID 
		  AND LAYOUT_ID = @LAYOUT_ID

END
GO