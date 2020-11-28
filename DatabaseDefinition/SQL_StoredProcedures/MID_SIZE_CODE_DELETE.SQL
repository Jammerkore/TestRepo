--dv =============================================
--dv Create date:	2/7/2014
--dv Description:	Deletes a Size Code RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CODE_DELETE]
	@SIZE_CODE_RID int

AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SIZE_CODE 
	WHERE
	SIZE_CODE_RID = @SIZE_CODE_RID 

	SELECT @@ROWCOUNT;
END
GO
