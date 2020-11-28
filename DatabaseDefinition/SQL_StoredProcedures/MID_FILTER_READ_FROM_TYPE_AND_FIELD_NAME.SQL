--dv =============================================
--dv Create date:	11/5/2014
--dv Description:	Reads filters based on filter type and a field name in the condition
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_READ_FROM_TYPE_AND_FIELD_NAME]
	@FILTER_TYPE INT,
	@FIELD_NAME VARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM FILTER f
	JOIN FILTER_CONDITION fc on fc.FILTER_RID = f.FILTER_RID
	WHERE f.FILTER_TYPE = @FILTER_TYPE
	  AND fc.VALUE_TO_COMPARE = @FIELD_NAME

END
GO

