--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads all rows from the audit reclass table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_RECLASS_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ph.START_TIME, 
		ph.PROCESS_ID, 
		at.TEXT_VALUE PROCESS_VALUE, 
		ar.RECLASS_SEQ, 
		ar.RECLASS_ACTION,
		ar.RECLASS_ITEM_TYPE, 
		ar.RECLASS_ITEM, 
		ar.RECLASS_COMMENT
	FROM AUDIT_RECLASS ar, 
		 APPLICATION_TEXT at, 
		 PROC_HDR ph
	WHERE ar.PROCESS_RID = ph.PROCESS_RID  
		  AND at.TEXT_CODE = ph.PROCESS_ID 
	ORDER BY ph.START_TIME, ar.RECLASS_SEQ

END
GO
