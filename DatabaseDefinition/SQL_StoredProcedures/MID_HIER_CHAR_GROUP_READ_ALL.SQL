--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads all rows from the hierarchy characteristic group table
--dv History: 4/29/2014 -jsobek -Corrected issue with procedure
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_GROUP_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HIER_CHAR_GROUP

END
GO
