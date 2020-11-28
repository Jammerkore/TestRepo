--dv =============================================
--dv Create date: 3/19/2015
--dv Description: Deletes a store characteristic group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_GROUP_DELETE]
	@SCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM STORE_CHAR_JOIN
	WHERE SC_RID IN (SELECT SC_RID FROM STORE_CHAR WHERE SCG_RID=@SCG_RID)

	DELETE 
	FROM STORE_CHAR
	WHERE SCG_RID = @SCG_RID

	DELETE
	FROM STORE_CHAR_GROUP
	WHERE SCG_RID = @SCG_RID

	SELECT @@ROWCOUNT
END
GO


