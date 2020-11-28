--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads all methods
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_ALL] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM VW_METHOD_READ

END
GO