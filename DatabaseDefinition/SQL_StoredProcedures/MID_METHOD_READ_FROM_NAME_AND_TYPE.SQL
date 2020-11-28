--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads the method based on the method name and method type id
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_FROM_NAME_AND_TYPE] 
	@METHOD_NAME varchar(50),
	@METHOD_TYPE_ID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM VW_METHOD_READ
	WHERE METHOD_NAME = @METHOD_NAME
	AND METHOD_TYPE_ID = @METHOD_TYPE_ID

END
GO