--dv =============================================
--dv Create date:	02/27/2015
--dv Description:	Reads Header Reconcile info based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_RECONCILE_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HEADER_RECONCILE_INFO WITH (NOLOCK)
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
