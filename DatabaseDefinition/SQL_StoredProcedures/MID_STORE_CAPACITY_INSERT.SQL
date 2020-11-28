--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Inserts a row into the store capacity table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CAPACITY_INSERT]
	@HN_RID int,
	@ST_RID int,
	@ST_CAPACITY int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO STORE_CAPACITY(HN_RID, ST_RID, ST_CAPACITY)
	VALUES (@HN_RID, @ST_RID, @ST_CAPACITY)

	SELECT @@ROWCOUNT;

END
GO
