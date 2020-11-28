--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads header rids from the user allocation header table based on the user rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ALLOCATION_HEADERS_READ]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		u.USER_RID, 
		vh.HDR_RID, 
		vh.Assortment 
	FROM USER_ALLOCATION_HEADERS u
	LEFT OUTER JOIN VW_GET_HEADERS vh on u.HDR_RID = vh.HDR_RID 
	WHERE u.USER_RID = @USER_RID

END
GO
