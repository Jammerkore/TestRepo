--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Inserts a row into the METHOD_BLD_PACKS_BPC_SELECT table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT]
	@METHOD_RID int,
	@BPC_COMBO_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_BLD_PACKS_BPC_SELECT 
		(
		METHOD_RID, 
		BPC_COMBO_RID
		)
    VALUES 
		(
		@METHOD_RID, 
		@BPC_COMBO_RID
		)

	SELECT @@ROWCOUNT;

END
GO