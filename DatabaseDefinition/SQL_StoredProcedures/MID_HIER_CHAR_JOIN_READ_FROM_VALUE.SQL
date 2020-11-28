--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads a row on the hierarchy characteristic join table based on the characteristic value rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_JOIN_READ_FROM_VALUE]
	@HC_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HIER_CHAR_JOIN hcj
    WHERE hcj.HC_RID = @HC_RID

END
GO
