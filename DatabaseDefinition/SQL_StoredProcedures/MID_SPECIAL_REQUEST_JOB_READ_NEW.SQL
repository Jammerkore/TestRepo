--dv =============================================
--dv Create date: 02/11/2014
--dv Description: Get a new specail request row.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_READ_NEW]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM SPECIAL_REQUEST_JOB
	WHERE SPECIAL_REQ_RID = - 1

END
GO


