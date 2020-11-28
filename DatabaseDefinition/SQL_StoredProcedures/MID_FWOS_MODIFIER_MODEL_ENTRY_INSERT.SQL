--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Inserts an entry for a forward weeks of supply (FWOS) modifier model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MODIFIER_MODEL_ENTRY_INSERT]
	@FWOSMOD_RID int,
	@FWOSMOD_SEQUENCE int,
	@FWOSMOD_VALUE float,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO FWOS_MODIFIER_MODEL_ENTRY(FWOSMOD_RID, FWOSMOD_SEQUENCE, FWOSMOD_VALUE, CDR_RID)
	VALUES (@FWOSMOD_RID, @FWOSMOD_SEQUENCE, @FWOSMOD_VALUE, @CDR_RID)

	SELECT @@ROWCOUNT;

END
GO
