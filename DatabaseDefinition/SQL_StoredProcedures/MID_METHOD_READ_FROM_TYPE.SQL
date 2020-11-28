--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads the method based on the method type id
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_FROM_TYPE] 
	@METHOD_TYPE_ID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM VW_METHOD_READ
	WHERE METHOD_TYPE_ID = @METHOD_TYPE_ID

END
GO