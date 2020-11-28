--dv =============================================
--dv Modified date:	12/18/2014
--dv Description:	Updates an assortment view
--dv History:		TT#1077 - MD - stodd - cannot create GA views
--dv History:		TT#4247 - stodd - add SG_RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_VIEW_UPDATE] 
	@VIEW_RID INT,
	@USER_RID INT,
	@VIEW_ID VARCHAR(250),
	@GROUP_BY_ID INT,
	@SG_RID INT,
	@ASSORTMENT_VIEW_TYPE INT
AS
BEGIN
	SET NOCOUNT ON

	UPDATE ASSORTMENT_VIEW
	SET
		USER_RID=@USER_RID,
		VIEW_ID=@VIEW_ID,
		GROUP_BY_ID=@GROUP_BY_ID,
		SG_RID=@SG_RID,
		ASSORTMENT_VIEW_TYPE=@ASSORTMENT_VIEW_TYPE
	WHERE VIEW_RID = @VIEW_RID

	SELECT @@ROWCOUNT;
END
GO


