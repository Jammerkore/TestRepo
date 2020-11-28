--dv =============================================
--dv Create date:	8/13/2014
--dv Description:	Returns the assortment views for the user list and view type
--dv History:		TT#1077 - MD - stodd - cannot create GA views
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_VIEW_READ_BY_TYPE]
	@USER_RID_LIST USER_RID_TYPE READONLY,
	@ASSORTMENT_VIEW_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		*
    FROM ASSORTMENT_VIEW 
	WHERE USER_RID in (SELECT USER_RID FROM @USER_RID_LIST)
		  AND ASSORTMENT_VIEW_TYPE = @ASSORTMENT_VIEW_TYPE
	ORDER BY VIEW_ID

END
GO