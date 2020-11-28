--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts an allocation task
--dv History:		9/29/2014 TT#1313-MD -jsobek -Header Filters
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_ALLOCATE_INSERT]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int, 
	@ALLOCATE_SEQUENCE int, 
	@HN_RID int, 
	@FILTER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TASK_ALLOCATE
		(
		TASKLIST_RID, 
		TASK_SEQUENCE, 
		ALLOCATE_SEQUENCE, 
		HN_RID, 
		FILTER_RID
		)
	VALUES
		(
		@TASKLIST_RID, 
		@TASK_SEQUENCE, 
		@ALLOCATE_SEQUENCE, 
		@HN_RID, 
		@FILTER_RID
		)

	SELECT @@ROWCOUNT;

END
GO