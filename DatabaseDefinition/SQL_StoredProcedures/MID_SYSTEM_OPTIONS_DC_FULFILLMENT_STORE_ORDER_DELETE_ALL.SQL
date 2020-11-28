--dv =============================================
--dv Create date:	05/09/2017
--dv Description:	Deletes all rows table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	TRUNCATE TABLE SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER

	SELECT @@ROWCOUNT;

END
GO
