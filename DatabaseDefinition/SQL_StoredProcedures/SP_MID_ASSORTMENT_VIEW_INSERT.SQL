--dv =============================================
--dv Modified date:	12/18/2014
--dv Description:	Inserts an assortment view
--dv History:		TT#1077 - MD - stodd - cannot create GA views
--dv History:		TT#4247 - stodd - add SG_RID
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_ASSORTMENT_VIEW_INSERT] 
	(
	@USER_RID INT,
	@VIEW_ID VARCHAR(250),
	@GROUP_BY_ID INT,
	@SG_RID INT,
	@ASSORTMENT_VIEW_TYPE INT,
	@VIEW_RID INT OUTPUT
	)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO ASSORTMENT_VIEW (
		USER_RID,
		VIEW_ID,
		GROUP_BY_ID,
		SG_RID,
		ASSORTMENT_VIEW_TYPE
		)
	VALUES (
		@USER_RID,
		@VIEW_ID,
		@GROUP_BY_ID,
		@SG_RID,
		@ASSORTMENT_VIEW_TYPE
		)

	SELECT @VIEW_RID = SCOPE_IDENTITY();
END
GO


