--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Inserts a modify sales method sell thru
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_SELL_THRU_INSERT] 
	@METHOD_RID int,
	@SELL_THRU int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_MOD_SALES_SELL_THRU
		(
		METHOD_RID, 
		SELL_THRU
		) 
	VALUES 
		(
		@METHOD_RID, 
		@SELL_THRU
		)

	SELECT @@ROWCOUNT;

END
GO
