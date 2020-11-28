--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes all purge criteria for the hierarchy node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_CRITERIA_DELETE_ALL]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PURGE_CRITERIA_HEADER_PURGE WITH (ROWLOCK) 
    WHERE HN_RID = @HN_RID

    DELETE 
	FROM PURGE_CRITERIA WITH (ROWLOCK)
    WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
