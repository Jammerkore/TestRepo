--dv =============================================
--dv Create date:	05/08/2017
--dv Description:	Deletes a system DCF
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE]
	@SYSTEM_OPTION_RID int,
	@SEQ int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER 
	WHERE SYSTEM_OPTION_RID = @SYSTEM_OPTION_RID 
		  AND SEQ = @SEQ

	SELECT @@ROWCOUNT;

END
GO