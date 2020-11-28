--dv =============================================
--dv Create date: 2/23/2015
--dv Description:	Get fields and characteristics for a store as well as current values for characteristic lists.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORES_READ_FIELDS_FOR_MAINT]
	@ST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	--Store field values
	SELECT 
		s.ST_RID,
		COALESCE(s.ST_ID, '') ST_ID,
		COALESCE(STORE_NAME, '') STORE_NAME,
		COALESCE(STORE_DESC, '') STORE_DESC,
		ACTIVE_IND,
		COALESCE(CITY, '') CITY,
		COALESCE(STATE, '') STATE,
		COALESCE(SELLING_SQ_FT, 0) SELLING_SQ_FT,
		SELLING_OPEN_DATE,
		SELLING_CLOSE_DATE,
		STOCK_OPEN_DATE,
		STOCK_CLOSE_DATE,
		COALESCE(LEAD_TIME, 0) LEAD_TIME,
		SHIP_ON_MONDAY,
		SHIP_ON_TUESDAY,
		SHIP_ON_WEDNESDAY,
		SHIP_ON_THURSDAY,
		SHIP_ON_FRIDAY,
		SHIP_ON_SATURDAY,
		SHIP_ON_SUNDAY,
		SIMILAR_STORE_MODEL,
		IMO_ID,
		STORE_DELETE_IND
	FROM STORES s
	WHERE s.ST_RID = @ST_RID

	--Store characteristic values for this store
	SELECT 
		--sc.SC_RID,
		scg.SCG_RID,
		scg.SCG_ID,
		scg.SCG_LIST_IND,
		scg.SCG_TYPE,
		(SELECT COALESCE(
		CASE 		
			WHEN scg.SCG_TYPE=1 THEN CONVERT(varchar(50), sc.DATE_VALUE, 101)
			WHEN scg.SCG_TYPE=2 THEN CONVERT(varchar(50), sc.NUMBER_VALUE)
			WHEN scg.SCG_TYPE=3 THEN CONVERT(varchar(50), sc.DOLLAR_VALUE)
			ELSE sc.TEXT_VALUE
		END, '')  
		FROM STORE_CHAR_JOIN scj 
			INNER JOIN STORE_CHAR sc ON sc.SC_RID = scj.SC_RID AND sc.SCG_RID = scg.SCG_RID AND scj.ST_RID = @ST_RID
		)  AS CHAR_VALUE,

		(SELECT
			sc.SC_RID 
		FROM STORE_CHAR_JOIN scj 
			INNER JOIN STORE_CHAR sc ON sc.SC_RID = scj.SC_RID AND sc.SCG_RID = scg.SCG_RID AND scj.ST_RID = @ST_RID
		)  AS SC_RID
	FROM STORE_CHAR_GROUP scg
	ORDER BY scg.SCG_ID


END
GO