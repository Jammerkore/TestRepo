--dv =============================================
--dv Create date: 02/07/2014
--dv Description: Get a special request job.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_READ]
	@JOB_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT srjj.*
	FROM SPECIAL_REQUEST_JOB srjt,
		SPECIAL_REQUEST_JOB_JOIN srjj,
		JOB jt
	WHERE srjt.SPECIAL_REQ_RID = srjj.SPECIAL_REQ_RID
		AND jt.JOB_RID = srjj.JOB_RID
		AND jt.JOB_RID = @JOB_RID

END
GO
