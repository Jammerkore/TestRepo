--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Updates a forward weeks of supply (FWOS) max model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MAX_MODEL_UPDATE]
	@FWOSMAX_RID int,
	@FWOSMAX_ID varchar(50),
	@FWOSMAX_DEFAULT float
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE FWOS_MAX_MODEL WITH (ROWLOCK)
	SET 
		FWOSMAX_ID = @FWOSMAX_ID,
		FWOSMAX_DEFAULT = @FWOSMAX_DEFAULT
	WHERE FWOSMAX_RID = @FWOSMAX_RID

	SELECT @@ROWCOUNT;

END
GO
