--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Returns all Assortment Views for RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_VIEW_READ_ALL]
	@USER_RID_LIST USER_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		*
    FROM ASSORTMENT_VIEW 
	WHERE USER_RID in (SELECT USER_RID FROM @USER_RID_LIST)

END
GO