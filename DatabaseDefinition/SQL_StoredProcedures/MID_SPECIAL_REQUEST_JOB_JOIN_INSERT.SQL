--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts a special request job join
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_JOIN_INSERT]
	@SPECIAL_REQ_RID int,
	@JOB_RID int,
	@JOB_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO SPECIAL_REQUEST_JOB_JOIN
		(
		SPECIAL_REQ_RID, 
		JOB_RID, 
		JOB_SEQUENCE
		)
	VALUES
		(
		@SPECIAL_REQ_RID, 
		@JOB_RID, 
		@JOB_SEQUENCE
		)

	SELECT @@ROWCOUNT;

END
GO