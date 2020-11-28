--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads chain plan set percentages for the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_SET_PERCENT_SET_READ]
	@HN_RID int,
	@BEGIN_WEEK int,
	@END_WEEK int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		sg.SG_RID AS SG_RID, 
		sg.SG_ID, 
		sg.SG_VERSION,  -- TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error. 
		sgl.SGL_RID AS SGL_RID, 
		sgl.SGL_ID, 
        PERCENTAGE, 
		(SELECT FISCAL_WEEK FROM FISCAL_WEEKS WHERE FIRST_DAY_OF_WEEK = csps.TIME_ID) AS TIME_ID
    FROM STORE_GROUP_LEVEL sgl 
		 INNER JOIN STORE_GROUP sg ON sgl.SG_RID = sg.SG_RID 
         INNER JOIN CHAIN_SET_PERCENT_SET csps ON sgl.SGL_RID = csps.SGL_RID 
    WHERE HN_RID= @HN_RID
          AND TIME_ID BETWEEN @BEGIN_WEEK AND @END_WEEK
    ORDER BY SGL_RID, TIME_ID

END
GO
