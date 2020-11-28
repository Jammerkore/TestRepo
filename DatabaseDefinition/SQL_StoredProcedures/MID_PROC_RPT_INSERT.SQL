--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Inserts a row into the audit table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PROC_RPT_INSERT]
	@PROCESS_RID int,
	@TIME_STAMP datetime,
	@REPORTING_MODULE varchar(250),
	@LINE_NUMBER int,
	@MESSAGE_LEVEL int,
	@MESSAGE_CODE int,
	@REPORT_MESSAGE varchar(5000)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PROC_RPT WITH (ROWLOCK)
		(
		PROCESS_RID, 
		TIME_STAMP, 
		REPORTING_MODULE, 
        LINE_NUMBER, 
		MESSAGE_LEVEL, 
		MESSAGE_CODE, 
		REPORT_MESSAGE
		) 
	VALUES 
		(
		@PROCESS_RID,
		@TIME_STAMP,
		@REPORTING_MODULE,
		@LINE_NUMBER,
		@MESSAGE_LEVEL,
		@MESSAGE_CODE,
		@REPORT_MESSAGE
		)

	SELECT @@ROWCOUNT;

END
GO
