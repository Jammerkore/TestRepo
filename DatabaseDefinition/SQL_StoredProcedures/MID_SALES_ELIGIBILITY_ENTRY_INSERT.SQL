--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row into the sales eligibility entry table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SALES_ELIGIBILITY_ENTRY_INSERT]
	@EM_RID int,
	@SALES_EM_SEQUENCE int,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SALES_ELIGIBILITY_ENTRY(EM_RID, SALES_EM_SEQUENCE, CDR_RID)
	VALUES (@EM_RID, @SALES_EM_SEQUENCE, @CDR_RID)

	SELECT @@ROWCOUNT;

END
GO
