--dv =============================================
--dv Create date:	6/25/2014
--dv Description:	Reads a filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_READ]
   	@FILTER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM FILTER
	WHERE FILTER_RID=@FILTER_RID

END
GO
