--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Reads the bulk color rule layer for the header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_COLOR_RULE_LAYER_READ]
	@HDR_BC_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM BULK_COLOR_RULE_LAYER
	WHERE HDR_BC_RID = @HDR_BC_RID

END
GO