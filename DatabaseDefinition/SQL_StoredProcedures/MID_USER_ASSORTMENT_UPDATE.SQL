--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Updates the User Assortment table 
--dv				when deleting a Assortment View
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ASSORTMENT_UPDATE]
	@VIEW_RID INT,
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON
	UPDATE USER_ASSORTMENT
	SET	VIEW_RID = @VIEW_RID
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO