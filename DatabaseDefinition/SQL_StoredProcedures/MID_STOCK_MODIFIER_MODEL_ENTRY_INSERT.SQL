--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Inserts a row into the stock modifier model entry table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MODIFIER_MODEL_ENTRY_INSERT]
	@STKMOD_RID int,
	@STKMOD_SEQUENCE int,
	@STKMOD_VALUE float,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO STOCK_MODIFIER_MODEL_ENTRY(STKMOD_RID, STKMOD_SEQUENCE, STKMOD_VALUE, CDR_RID)
	VALUES (@STKMOD_RID, @STKMOD_SEQUENCE, @STKMOD_VALUE, @CDR_RID)

	SELECT @@ROWCOUNT;

END
GO
