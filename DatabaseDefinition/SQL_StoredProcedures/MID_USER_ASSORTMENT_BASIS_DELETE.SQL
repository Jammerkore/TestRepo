--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Deletes Assortment User Basis
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ASSORTMENT_BASIS_DELETE]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM USER_ASSORTMENT_BASIS 
	WHERE
	USER_RID = @USER_RID
	 
	SELECT @@ROWCOUNT;
END
GO