--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Returns all Assortment Views for RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_VIEW_READ_VIEW_RID]
	@VIEW_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		*
    FROM ASSORTMENT_VIEW 
	WHERE VIEW_RID = @VIEW_RID

END
GO