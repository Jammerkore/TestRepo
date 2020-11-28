--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Inserts an entry for a forward weeks of supply (FWOS) max model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MAX_MODEL_ENTRY_INSERT]
	@FWOSMAX_RID int,
	@FWOSMAX_SEQUENCE int,
	@FWOSMAX_VALUE float,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO FWOS_MAX_MODEL_ENTRY(FWOSMAX_RID, FWOSMAX_SEQUENCE, FWOSMAX_VALUE, CDR_RID)
    VALUES (@FWOSMAX_RID, @FWOSMAX_SEQUENCE, @FWOSMAX_VALUE, @CDR_RID)

	SELECT @@ROWCOUNT;

END
GO
