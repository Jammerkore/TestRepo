--dv =============================================
--dv Create date: 5/4/2015
--dv Description: Removes any old entries from the join table for the group and inserts a store char join entry
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_JOIN_DELETE_FOR_GROUP_AND_STORE] 
	@SCG_RID INT,
	@ST_RID INT
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT * FROM STORE_CHAR_JOIN WHERE ST_RID=@ST_RID AND SC_RID IN
		(SELECT SC_RID FROM STORE_CHAR sc WHERE sc.SCG_RID=@SCG_RID))
	BEGIN
		DELETE FROM STORE_CHAR_JOIN WHERE ST_RID=@ST_RID AND SC_RID IN (SELECT SC_RID FROM STORE_CHAR sc WHERE sc.SCG_RID=@SCG_RID)
	END



	SELECT @@ROWCOUNT; 
END
GO


