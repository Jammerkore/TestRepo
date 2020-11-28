--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Read the product hierarchy RIDs for the user passed in
--dv For Review:	Should the inner join come before the left outer join??
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRODUCT_HIERARCHY_READ] 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @PROFILE_HIERARCHY int = 26;

	SELECT 
		ph.PH_RID PH_RID, 
		ph.PH_ID PH_ID,
        ph.PH_TYPE PH_TYPE, 
		ph.PH_COLOR PH_COLOR,
        ph.PH_OWNER PH_OWNER, 
		pd.POSTING_DATE POSTING_DATE, 
		ph.OTS_PLANLEVEL_TYPE, 
		ph.HISTORY_ROLL_OPTION,
        ui.USER_RID, 
		ui.ITEM_TYPE, 
		ui.ITEM_RID, 
		ui.OWNER_USER_RID
	FROM PRODUCT_HIERARCHY ph 
		 LEFT OUTER JOIN POSTING_DATE pd ON ph.PH_RID = pd.PH_RID
         INNER JOIN USER_ITEM ui ON ui.ITEM_RID = ph.PH_RID 
	WHERE ui.ITEM_TYPE = @PROFILE_HIERARCHY

END
GO
