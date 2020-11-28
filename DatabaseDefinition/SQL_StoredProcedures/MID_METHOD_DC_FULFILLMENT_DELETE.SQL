--dv =============================================
--dv Modified date: 4/18/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith- DC Fulfillment
--dv ============================================= 

CREATE PROCEDURE [dbo].[MID_METHOD_DC_FULFILLMENT_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_DC_FULFILLMENT_STORE_ORDER WITH (ROWLOCK)
	WHERE METHOD_RID = @METHOD_RID

	DELETE 
	FROM METHOD_DC_FULFILLMENT WITH (ROWLOCK)
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
	 
END
GO









