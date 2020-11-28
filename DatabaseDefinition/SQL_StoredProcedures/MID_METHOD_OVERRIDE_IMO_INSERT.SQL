--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Inserts an IMO override method for a store
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_IMO_INSERT]
	@METHOD_RID int, 
	@ST_RID int, 
	@IMO_MIN_SHIP_QTY int, 
	@IMO_PCT_PK_THRSHLD float, 
	@IMO_MAX_VALUE int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_OVERRIDE_IMO 
		(
		METHOD_RID, 
		ST_RID, 
		IMO_MIN_SHIP_QTY, 
		IMO_PCT_PK_THRSHLD, 
		IMO_MAX_VALUE
		)
	VALUES
		(
		@METHOD_RID, 
		@ST_RID, 
		@IMO_MIN_SHIP_QTY, 
		@IMO_PCT_PK_THRSHLD, 
		@IMO_MAX_VALUE
		)

	SELECT @@ROWCOUNT;
END
GO