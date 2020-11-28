--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Deletes a message
--dv =============================================
CREATE PROCEDURE [dbo].[MID_MESSAGE_QUEUE_DELETE]
	@MESSAGE_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE
    FROM MESSAGE_QUEUE WITH (ROWLOCK)
    WHERE MESSAGE_RID = @MESSAGE_RID 

	SELECT @@ROWCOUNT;

END
GO


