--dv =============================================
--dv Create date: 2/23/2015
--dv Description:	Get current values for characteristic lists.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_GROUP_READ_FOR_MAINT]
AS
BEGIN
	SET NOCOUNT ON;

	--Store characteristic list values (for list types)
	SELECT 
		sc.SCG_RID,
		sc.SC_RID,
		CASE 		
			WHEN scg.SCG_TYPE=1 THEN CONVERT(varchar(50), sc.DATE_VALUE, 101)
			WHEN scg.SCG_TYPE=2 THEN CONVERT(varchar(50), sc.NUMBER_VALUE)
			WHEN scg.SCG_TYPE=3 THEN CONVERT(varchar(50), sc.DOLLAR_VALUE)
			ELSE sc.TEXT_VALUE
		END AS CHAR_LIST_VALUE
	FROM STORE_CHAR sc 
	INNER JOIN STORE_CHAR_GROUP scg ON scg.SCG_RID = sc.SCG_RID
	WHERE scg.SCG_LIST_IND = 1
	ORDER BY scg.SCG_RID, CHAR_LIST_VALUE


END
GO