--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Updates a stock modifer model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MODIFIER_MODEL_UPDATE]
	@STKMOD_RID int,
	@STKMOD_ID varchar(50),
	@STKMOD_DEFAULT float
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE STOCK_MODIFIER_MODEL WITH (ROWLOCK)
	SET 
		STKMOD_ID = @STKMOD_ID,
		STKMOD_DEFAULT = @STKMOD_DEFAULT
	WHERE STKMOD_RID = @STKMOD_RID

	SELECT @@ROWCOUNT;

END
GO
