--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Inserts a row into the priority shipping entry table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRIORITY_SHIPPING_ENTRY_INSERT]
	@EM_RID int,
	@PRI_SHIP_EM_SEQUENCE int,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PRIORITY_SHIPPING_ENTRY(EM_RID, PRI_SHIP_EM_SEQUENCE, CDR_RID)
	VALUES (@EM_RID, @PRI_SHIP_EM_SEQUENCE, @CDR_RID)

	SELECT @@ROWCOUNT;

END
GO
