--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Deletes an entry for a forward weeks of supply (FWOS) modifier model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MODIFIER_MODEL_ENTRY_DELETE]
	@FWOSMOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM FWOS_MODIFIER_MODEL_ENTRY WITH (ROWLOCK)
	WHERE FWOSMOD_RID = @FWOSMOD_RID

	SELECT @@ROWCOUNT;

END
GO
