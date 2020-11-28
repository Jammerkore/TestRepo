--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row into the stock eligibility entry table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_ELIGIBILITY_ENTRY_INSERT]
	@EM_RID int,
	@STOCK_EM_SEQUENCE int,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO STOCK_ELIGIBILITY_ENTRY(EM_RID, STOCK_EM_SEQUENCE, CDR_RID)
	VALUES (@EM_RID, @STOCK_EM_SEQUENCE, @CDR_RID)

	SELECT @@ROWCOUNT;

END
GO
