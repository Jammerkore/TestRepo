--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads all rows from the hierarchy characteristic group table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_GROUP_VALUES_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HIER_CHAR hc, HIER_CHAR_GROUP hcg
	WHERE hc.HCG_RID = hcg.HCG_RID
	ORDER BY hcg.HCG_ID, hc.HC_ID

END
GO
