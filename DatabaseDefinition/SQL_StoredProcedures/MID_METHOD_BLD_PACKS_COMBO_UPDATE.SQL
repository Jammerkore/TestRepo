--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Inserts a row into the METHOD_BLD_PACKS_BPC_SELECT table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_BLD_PACKS_COMBO_UPDATE]
	@METHOD_RID int,
	@COMBO_RID int,
	@COMBO_NAME varchar(50),
	@COMBO_SELECTED_IND char(1)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE METHOD_BLD_PACKS_COMBO 
	SET
		COMBO_NAME = @COMBO_NAME,
		COMBO_SELECTED_IND = @COMBO_SELECTED_IND
	WHERE METHOD_RID = @METHOD_RID
		  AND COMBO_RID = @COMBO_RID

	SELECT @@ROWCOUNT;

END
GO