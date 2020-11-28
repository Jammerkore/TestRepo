--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Deletes all processed rows in the computation item table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_ITEM_DELETE_ALL_PROCESSED]
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM COMPUTATION_ITEM 
	WHERE ITEM_PROCESSED IS NOT NULL

	SELECT @@ROWCOUNT;

END
GO
