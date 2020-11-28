--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Updates a forward weeks of supply (FWOS) modifier model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MODIFIER_MODEL_UPDATE]
	@FWOSMOD_RID int,
	@FWOSMOD_ID varchar(50),
	@FWOSMOD_DEFAULT float
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE FWOS_MODIFIER_MODEL WITH (ROWLOCK)
	SET 
		FWOSMOD_ID = @FWOSMOD_ID,
		FWOSMOD_DEFAULT = @FWOSMOD_DEFAULT
	WHERE FWOSMOD_RID = @FWOSMOD_RID

	SELECT @@ROWCOUNT;

END
GO
