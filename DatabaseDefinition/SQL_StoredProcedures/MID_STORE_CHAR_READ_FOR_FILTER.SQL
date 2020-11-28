--dv =============================================
--dv Create date: 7/11/2014
--dv Description: Returns all stores rids along with the characteristic value for a specific characteristic group
--dv Example:  Store characteristic group=Climate
--dv		   Store characteristic values=Hot, Cold
--dv				public enum eStoreCharType
--dv				{
--dv					text=0,
--dv					date=1,
--dv					number=2,
--dv					dollar=3,
--dv					unknown=4,
--dv					list = 5, --not supported
--dv					boolean = 6 --not supported
--dv				}
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_READ_FOR_FILTER]
	@SCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @STORE_CHAR_GROUP_VALUE_TYPE INT;

	SELECT @STORE_CHAR_GROUP_VALUE_TYPE = SCG_TYPE FROM STORE_CHAR_GROUP WHERE SCG_RID=@SCG_RID

	SELECT
		SC.SCG_RID,
		SC.SC_RID,
		SCJ.ST_RID,
		CASE 		
			WHEN @STORE_CHAR_GROUP_VALUE_TYPE=1 THEN CONVERT(varchar(50), SC.DATE_VALUE, 101)
			WHEN @STORE_CHAR_GROUP_VALUE_TYPE=2 THEN CONVERT(varchar(50), SC.NUMBER_VALUE)
			WHEN @STORE_CHAR_GROUP_VALUE_TYPE=3 THEN CONVERT(varchar(50), SC.DOLLAR_VALUE)
			ELSE SC.TEXT_VALUE
		END AS CHAR_VALUE
		
	FROM STORE_CHAR SC WITH (NOLOCK)
	INNER JOIN STORE_CHAR_JOIN SCJ WITH (NOLOCK) ON SCJ.SC_RID=SC.SC_RID
	WHERE SC.SCG_RID=@SCG_RID

END

GO


