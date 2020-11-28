--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads store capacity based on the hierarchy node rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CAPACITY_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HN_RID, 
		ST_RID, 
		COALESCE(ST_CAPACITY, -1) ST_CAPACITY 
	FROM STORE_CAPACITY 
	WHERE HN_RID = @HN_RID

END
GO
