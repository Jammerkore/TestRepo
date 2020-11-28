--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Deletes any size group joins
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_GROUP_JOIN_DELETE]
	@SIZE_GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_GROUP_JOIN
	WHERE SIZE_GROUP_RID = @SIZE_GROUP_RID

	SELECT @@ROWCOUNT;
END
GO
