--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads size basis substitutes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM METHOD_SIZE_BASIS_SUBSTITUTES 
	WHERE METHOD_RID = @METHOD_RID

END
GO