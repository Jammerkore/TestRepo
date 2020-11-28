--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Get all orphaned system tasklist.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TASKLIST_RID,
		SYSTEM_GENERATED_IND TASKLIST_SYSTEM_GENERATED_IND
	FROM TASKLIST
	WHERE TASKLIST_RID NOT IN (
			SELECT TASKLIST_RID
			FROM JOB_TASKLIST_JOIN
			)
		AND SYSTEM_GENERATED_IND = '1'
END
GO

