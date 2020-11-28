--dv =============================================
--dv Create date:	2/6/2014
--dv Description:	Reads a count of rules for the header and layer provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DETAIL_RULE_READ_COUNT_FROM_HEADER_AND_LAYER]
	@HDR_RID INT,
	@LAYER_ID INT,
	@ST_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		COUNT(*) MyCount 
	FROM DETAIL_RULE 
	WHERE HDR_RID = @HDR_RID 
		  AND LAYER_ID = @LAYER_ID				
		  AND ST_RID = @ST_RID

END
GO