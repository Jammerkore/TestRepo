--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Reads the bulk rule layer for the header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_RULE_LAYER_READ]
	@HDR_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM BULK_RULE_LAYER
	WHERE HDR_RID = @HDR_RID

END
GO