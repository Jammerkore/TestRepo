--dv =============================================
--dv Create date:	6/25/2014
--dv Description:	Reads all condition list values for a filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_CONDITION_LIST_VALUES_READ]
   	@FILTER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT LV.*
	FROM FILTER_CONDITION FC
	INNER JOIN FILTER_CONDITION_LIST_VALUES LV ON LV.CONDITION_RID = FC.CONDITION_RID
	WHERE FC.FILTER_RID=@FILTER_RID

END
GO
