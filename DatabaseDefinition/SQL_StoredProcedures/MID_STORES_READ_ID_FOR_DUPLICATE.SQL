--dv =============================================
--dv Create date: 3/17/2015
--dv Description:	Returns a count of rows that match the proposed store id
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORES_READ_ID_FOR_DUPLICATE]
	@PROPOSED_STORE_ID VARCHAR(50),
	@ST_EDIT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*) AS MYCOUNT
	FROM STORES
	WHERE ST_ID=@PROPOSED_STORE_ID
		AND ST_RID <> @ST_EDIT_RID


END
GO