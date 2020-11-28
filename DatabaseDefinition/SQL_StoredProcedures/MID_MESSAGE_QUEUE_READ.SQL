--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Reads messages
--dv =============================================
CREATE PROCEDURE [dbo].[MID_MESSAGE_QUEUE_READ] 
	@MESSAGE_TO int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		MESSAGE_RID, 
		MESSAGE_DATETIME, 
		MESSAGE_TO,
        MESSAGE_FROM, 
		MESSAGE_CODE, 
		MESSAGE_PROCESSING_PRIORITY, 
		MESSAGE_DETAILS
    FROM MESSAGE_QUEUE
    WHERE MESSAGE_TO = @MESSAGE_TO
    ORDER BY MESSAGE_PROCESSING_PRIORITY

END
GO