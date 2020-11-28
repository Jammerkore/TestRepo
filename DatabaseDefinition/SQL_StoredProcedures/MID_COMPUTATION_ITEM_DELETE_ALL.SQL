--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Deletes all rows in the computation item table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_ITEM_DELETE_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	TRUNCATE TABLE COMPUTATION_ITEM

	SELECT @@ROWCOUNT;

END
GO
