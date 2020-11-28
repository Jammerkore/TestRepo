--dv =============================================
--dv Create date:	6/25/2014
--dv Description:	Deletes filter conditions for a filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_CONDITION_DELETE]
   	@FILTER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM FILTER_CONDITION
	WHERE FILTER_RID=@FILTER_RID

	SELECT @@ROWCOUNT

END
GO
