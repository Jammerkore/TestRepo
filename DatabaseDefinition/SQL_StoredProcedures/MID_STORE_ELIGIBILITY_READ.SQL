--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads store eligibility for the given hierarchy node rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_ELIGIBILITY_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		se.HN_RID, 
		se.ST_RID, 
		COALESCE(se.EM_RID, -1) EM_RID,
        se.USE_ELIGIBILITY,
        COALESCE(se.INELIGIBLE, '1') INELIGIBLE,
        COALESCE(se.STKMOD_TYPE, 0) STKMOD_TYPE, 
		COALESCE(se.STKMOD_RID, -1) STKMOD_RID, 
		se.STKMOD_PCT,
        COALESCE(se.SLSMOD_TYPE, 0) SLSMOD_TYPE, 
		COALESCE(se.SLSMOD_RID, -1) SLSMOD_RID, 
		se.SLSMOD_PCT,
        COALESCE(se.FWOSMOD_TYPE, 0) FWOSMOD_TYPE, 
		COALESCE(se.FWOSMOD_RID, -1) FWOSMOD_RID, 
		se.FWOSMOD_PCT,
        COALESCE(se.SIMILAR_STORE_TYPE, 0) SIMILAR_STORE_TYPE,
        se.SIMILAR_STORE_RATIO, 
		COALESCE(se.UNTIL_DATE, -1) UNTIL_DATE, 
        se.PRESENTATION_PLUS_SALES_IND,
        cdr.CDR_RID, 
		cdr.CDR_NAME, 
		cdr.CDR_START, 
		cdr.CDR_END, 
		cdr.CDR_RANGE_TYPE_ID, 
		cdr.CDR_DATE_TYPE_ID, 
		cdr.CDR_RELATIVE_TO,
        se.STOCK_LEAD_WEEKS
	FROM STORE_ELIGIBILITY se 
		 LEFT OUTER JOIN CALENDAR_DATE_RANGE cdr ON se.UNTIL_DATE = cdr.CDR_RID
	WHERE HN_RID=@HN_RID
    
END
GO
