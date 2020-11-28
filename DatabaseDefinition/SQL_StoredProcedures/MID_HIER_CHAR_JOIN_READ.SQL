--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads a row on the hierarchy characteristic join table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_JOIN_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HIER_CHAR_JOIN hcj 
	WHERE hcj.HN_RID = @HN_RID

END
GO
