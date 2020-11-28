--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads imo data for stores based on the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_IMO_READ_FOR_STORES]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		st.ST_RID, 
		imo.HN_RID, 
		imo.IMO_MAX_VALUE, 
		imo.IMO_FWOS_MAX, 
		imo.IMO_MIN_SHIP_QTY, 
		imo.IMO_PCT_PK_THRSHLD, 
		imo.IMO_PSH_BK_STK, 
		imo.IMO_FWOS_MAX_RID, 
		imo.IMO_FWOS_MAX_TYPE 
	FROM STORES st 
	LEFT OUTER JOIN NODE_IMO imo ON st.ST_RID = imo.ST_RID 
    WHERE HN_RID = @HN_RID

END
GO
