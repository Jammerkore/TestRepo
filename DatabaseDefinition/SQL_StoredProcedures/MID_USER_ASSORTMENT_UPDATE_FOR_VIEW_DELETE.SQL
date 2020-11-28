--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Updates the User Assortment table 
--dv				when deleting a Assortment View
--dv History:		TT#1268-MD -jsobek -5.4 Merge
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE]
	@VIEW_RID INT,
	@DEFAULT_VIEW_RID INT
AS
BEGIN
	SET NOCOUNT ON

	UPDATE ASSORTMENT_USER_VIEW_JOIN 
		SET VIEW_RID = @DEFAULT_VIEW_RID 
	WHERE VIEW_RID = @VIEW_RID

	UPDATE USER_ASSORTMENT
	SET	VIEW_RID = @DEFAULT_VIEW_RID
	WHERE VIEW_RID = @VIEW_RID

	SELECT @@ROWCOUNT;

END
GO