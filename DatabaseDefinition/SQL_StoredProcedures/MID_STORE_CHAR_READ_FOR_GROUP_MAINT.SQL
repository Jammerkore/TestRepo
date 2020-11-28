--dv =============================================
--dv Create date: 3/18/2015
--dv Description: Reads all possible store characteristic values from a store characteristic group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_READ_FOR_GROUP_MAINT]
	@SCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	--Store characteristic values for this group
	SELECT 
		sc.SC_RID,
		scg.SCG_RID,
		scg.SCG_ID,
		scg.SCG_LIST_IND,
		scg.SCG_TYPE,
		CASE 		
			WHEN scg.SCG_TYPE=1 THEN CONVERT(varchar(50), sc.DATE_VALUE, 101)
			WHEN scg.SCG_TYPE=2 THEN CONVERT(varchar(50), sc.NUMBER_VALUE)
			WHEN scg.SCG_TYPE=3 THEN CONVERT(varchar(50), sc.DOLLAR_VALUE)
			ELSE sc.TEXT_VALUE
		END  AS CHAR_VALUE

	FROM STORE_CHAR_GROUP scg
	INNER JOIN STORE_CHAR sc ON sc.SCG_RID = scg.SCG_RID 
	WHERE scg.SCG_RID=@SCG_RID AND scg.SCG_LIST_IND='1'
	ORDER BY CHAR_VALUE

END

GO


