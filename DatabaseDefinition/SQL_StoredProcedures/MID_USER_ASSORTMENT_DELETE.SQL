--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Deletes a User Assortment
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ASSORTMENT_DELETE]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM USER_ASSORTMENT
	WHERE
	USER_RID = @USER_RID
	 
	SELECT @@ROWCOUNT;
END
GO