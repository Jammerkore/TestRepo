--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Inserts a row on the similar stores table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIMILAR_STORES_INSERT]
	@HN_RID int,
	@ST_RID int,
	@SS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SIMILAR_STORES(HN_RID, ST_RID, SS_RID)
	VALUES (@HN_RID, @ST_RID, @SS_RID)

	SELECT @@ROWCOUNT;

END
GO
