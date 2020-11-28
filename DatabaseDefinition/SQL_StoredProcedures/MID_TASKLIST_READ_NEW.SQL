--dv =============================================
--dv Create date: 02/11/2014
--dv Description: Get new tasklist row.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASKLIST_READ_NEW]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM TASKLIST
	WHERE TASKLIST_RID = - 1

END
GO


