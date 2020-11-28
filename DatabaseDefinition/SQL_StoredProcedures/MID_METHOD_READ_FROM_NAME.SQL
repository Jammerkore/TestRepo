--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads the method based on the method name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_FROM_NAME] 
	@METHOD_NAME varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM VW_METHOD_READ
	WHERE METHOD_NAME = @METHOD_NAME

END
GO