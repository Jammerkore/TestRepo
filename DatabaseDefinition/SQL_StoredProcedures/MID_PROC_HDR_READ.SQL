--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads from the process audit header table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PROC_HDR_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ph.PROCESS_RID AS 'Process RID', 
		ph.PROCESS_ID AS 'Process ID',
		at1.TEXT_VALUE AS 'Process',
		ph.COMPLETION_STATUS_CODE AS 'Completion Status Code', 
		at2.TEXT_VALUE AS 'Completion Status',
		ph.EXECUTION_STATUS_CODE AS 'Execution Status Code', 
		at3.TEXT_VALUE AS 'Execution Status',
		ph.START_TIME AS 'Start Time', 
		ph.STOP_TIME AS 'Stop Time',
		ph.USER_RID AS 'User RID', 
		au.USER_NAME AS 'User Name',
		at4.TEXT_VALUE AS 'Highest Message Level',
		ph.SUMMARY_CODE AS 'Summary Code', 
		at4.TEXT_VALUE AS 'Summary',
		ph.PROC_DESC AS 'Description'
	FROM PROC_HDR ph, 
		 APPLICATION_TEXT at1, 
		 APPLICATION_TEXT at2, 
		 APPLICATION_TEXT at3,
		 APPLICATION_TEXT at4, 
		 APPLICATION_TEXT at5, 
		 APPLICATION_USER au
	WHERE PROCESS_RID = @PROCESS_RID
		  AND ph.PROCESS_ID = at1.TEXT_CODE 
		  AND ph.COMPLETION_STATUS_CODE = at2.TEXT_CODE 
		  AND ph.EXECUTION_STATUS_CODE = at3.TEXT_CODE 
		  AND ph.SUMMARY_CODE = at4.TEXT_CODE 
		  AND ph.HIGHEST_LEVEL = at5.TEXT_CODE 
		  AND ph.USER_RID = au.USER_RID

END
GO
