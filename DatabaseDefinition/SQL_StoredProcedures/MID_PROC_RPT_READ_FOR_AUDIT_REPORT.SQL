--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads from the PROC_RPT table for audit reporting
--dv For Review:	Test for no lock and concurrency
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PROC_RPT_READ_FOR_AUDIT_REPORT]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		pr.PROCESS_RID AS 'ProcessRID',
		pr.TIME_STAMP AS 'Time',
		pr.REPORTING_MODULE AS 'Module',  
		pr.LINE_NUMBER AS 'LineNumber',
		pr.MESSAGE_LEVEL AS 'MessageLevelCode', 
		at2.TEXT_VALUE AS 'MessageLevel',
		pr.MESSAGE_CODE AS 'MessageCode', 
		at.TEXT_VALUE AS 'Message',
		COALESCE(pr.REPORT_MESSAGE,' ') AS 'ReportMessage'
    FROM PROC_RPT pr WITH (NOLOCK) 
		 LEFT OUTER JOIN APPLICATION_TEXT at WITH (NOLOCK) ON pr.MESSAGE_CODE = at.TEXT_CODE 
		 LEFT OUTER JOIN APPLICATION_TEXT at2 WITH (NOLOCK) ON pr.MESSAGE_LEVEL = at2.TEXT_CODE
	WHERE PROCESS_RID = @PROCESS_RID	  
	ORDER BY pr.TIME_STAMP DESC

END
GO
