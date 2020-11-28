--dv =============================================
--dv Create date:	05/13/2015
--dv Description:	Reads rows fromm the Batch Comp Selection Filter Table based on the batch comp rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BATCH_COMP_SELECTION_FILTER_READ]
	@BATCH_COMP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM BATCH_COMP_SELECTION_FILTER
	WHERE BATCH_COMP_RID = @BATCH_COMP_RID

END
GO