--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Deletes a forward weeks of supply (FWOS) max model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MAX_MODEL_DELETE]
	@FWOSMAX_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM FWOS_MAX_MODEL_ENTRY WITH (ROWLOCK) 
    WHERE FWOSMAX_RID = @FWOSMAX_RID
               
	DELETE 
	FROM FWOS_MAX_MODEL WITH (ROWLOCK)
    WHERE FWOSMAX_RID = @FWOSMAX_RID

	SELECT @@ROWCOUNT;

END
GO
