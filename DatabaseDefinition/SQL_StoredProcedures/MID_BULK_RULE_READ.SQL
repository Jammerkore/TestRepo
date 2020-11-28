--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Reads the rule info
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BULK_RULE_READ]
	@HDR_RID INT,
	@LAYER_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM BULK_RULE
	WHERE HDR_RID = @HDR_RID
	AND LAYER_ID = @LAYER_ID

END
GO