--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads the method based on the method rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM VW_METHOD_READ
	WHERE METHOD_RID = @METHOD_RID

END
GO