--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Deletes store capacity rows based on the hierarchy node rid and store rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CAPACITY_DELETE]
	@HN_RID int,
	@ST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_CAPACITY WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
		  AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;

END
GO
