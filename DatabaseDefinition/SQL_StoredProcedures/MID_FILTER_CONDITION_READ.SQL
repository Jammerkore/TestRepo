--dv =============================================
--dv Create date:	6/25/2014
--dv Description:	Reads all conditions filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_CONDITION_READ]
   	@FILTER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM FILTER_CONDITION
	WHERE FILTER_RID=@FILTER_RID
	ORDER BY PARENT_SEQ, SIBLING_SEQ

END
GO
