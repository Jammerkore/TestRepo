--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads all rows from the posting date table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_POSTING_DATE_READ_ALL] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM POSTING_DATE

END
GO
