--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Returns a Assortment View Detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_VIEW_DETAIL_READ]
	@VIEW_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
    FROM ASSORTMENT_VIEW_DETAIL 
	WHERE VIEW_RID = @VIEW_RID
	ORDER BY 
		VIEW_RID, 
		AXIS, 
		AXIS_SEQUENCE
END
GO