--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Inserts a sales modifier model entry
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SALES_MODIFIER_MODEL_ENTRY_INSERT]
	@SLSMOD_RID int,
	@SLSMOD_SEQUENCE int,
	@SLSMOD_VALUE float,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SALES_MODIFIER_MODEL_ENTRY(SLSMOD_RID, SLSMOD_SEQUENCE, SLSMOD_VALUE, CDR_RID)
	VALUES (@SLSMOD_RID, @SLSMOD_SEQUENCE, @SLSMOD_VALUE, @CDR_RID)

	SELECT @@ROWCOUNT;

END
GO
