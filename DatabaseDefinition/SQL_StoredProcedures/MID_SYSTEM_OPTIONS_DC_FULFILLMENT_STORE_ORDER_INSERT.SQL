--dv =============================================
--dv Create date:	05/08/2017
--dv Description:	Inserts a system basis label
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT]
	@SYSTEM_OPTION_RID int,
	@SEQ int, 
	@DIST_CENTER varchar(50), 
	@SCG_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER
		(
		SYSTEM_OPTION_RID, 
		SEQ, 
		DIST_CENTER,
		SCG_RID
		)
    VALUES
		(
		@SYSTEM_OPTION_RID, 
		@SEQ, 
		@DIST_CENTER,
		@SCG_RID
		)

	SELECT @@ROWCOUNT;

END
GO