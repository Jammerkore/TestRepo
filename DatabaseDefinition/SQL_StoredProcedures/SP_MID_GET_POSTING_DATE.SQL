--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Reads the posting date
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_GET_POSTING_DATE] 
	@postday INT OUTPUT,
	@nextday INT OUTPUT,
	@Return_Code INT = - 100 OUTPUT,
	@debug BIT = 0
AS
BEGIN
	IF (@debug = 0)
		SET NOCOUNT ON

	SELECT @postday = pd.POSTING_DATE_YYYYDDD,
		@nextday = CURR_DATE_YYYYDDD
	FROM POSTING_DATE pd,
		PRODUCT_HIERARCHY ph
	WHERE ph.PH_TYPE = 800000
		AND pd.PH_RID = ph.PH_RID

	SET @Return_Code = 0

	RETURN @Return_Code
END
GO


