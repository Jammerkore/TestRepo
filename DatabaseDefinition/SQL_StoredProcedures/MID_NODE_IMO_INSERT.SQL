--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row into the node imo table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_IMO_INSERT]
	@HN_RID int,
	@ST_RID int,
	@IMO_MIN_SHIP_QTY int,
	@IMO_PCT_PK_THRSHLD float,
	@IMO_FWOS_MAX float,
	@IMO_MAX_VALUE int,
	@IMO_PSH_BK_STK int,
	@IMO_FWOS_MAX_RID int,
	@IMO_FWOS_MAX_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO NODE_IMO (HN_RID, ST_RID, IMO_MIN_SHIP_QTY, IMO_PCT_PK_THRSHLD, IMO_FWOS_MAX, IMO_MAX_VALUE, IMO_PSH_BK_STK, IMO_FWOS_MAX_RID, IMO_FWOS_MAX_TYPE)
	VALUES (@HN_RID, @ST_RID, @IMO_MIN_SHIP_QTY, @IMO_PCT_PK_THRSHLD, @IMO_FWOS_MAX, @IMO_MAX_VALUE, @IMO_PSH_BK_STK, @IMO_FWOS_MAX_RID, @IMO_FWOS_MAX_TYPE)

	SELECT @@ROWCOUNT;

END
GO
