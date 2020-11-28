--dv Create date:	9/3/2013
--dv Description:	Gets similar store RIDS for a group of stores based on HN_RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GET_SIMILAR_STORE_RIDS] 
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT HN_RID, ss.ST_RID, SS_RID 
	FROM SIMILAR_STORES ss
	WHERE HN_RID=@HN_RID

END
GO
