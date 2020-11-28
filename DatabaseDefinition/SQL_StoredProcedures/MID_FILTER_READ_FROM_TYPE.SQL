--dv =============================================
--dv Create date:	11/5/2014
--dv Description:	Reads filters based on filter type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_READ_FROM_TYPE]
	@FILTER_TYPE INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM FILTER f
	WHERE f.FILTER_TYPE = @FILTER_TYPE

END
GO

