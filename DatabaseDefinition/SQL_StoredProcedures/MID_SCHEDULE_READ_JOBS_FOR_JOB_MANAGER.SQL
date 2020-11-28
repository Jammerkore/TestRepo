--dv =============================================
--dv Create date: 07/02/2015
--dv Description: Get all active scheduled jobs.
--dv stodd - added parameters 09/16/2015
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER]
@SCHED_NAME varchar(100),
@JOB_NAME varchar(100),
@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

/* For Testing */
--Declare @SCHED_NAME varchar(100),
--@JOB_NAME varchar(100),
--@USER_RID int
--set @SCHED_NAME = ''
--set @JOB_NAME = 'Weekly Rollup'
--set @USER_RID = 129
/* End For Testing */

	DECLARE @eProcessExecutionStatus_Queued INT = 801610;
	DECLARE @eProcessExecutionStatus_Waiting INT = 801601;
	DECLARE @eProcessExecutionStatus_Running INT = 801602;
	DECLARE @eProcessExecutionStatus_OnHold INT = 801603;
	DECLARE @eProcessExecutionStatus_Executed INT = 801606;

	/*==========*/
	/* ALL JOBS */
	/*==========*/
	IF @JOB_NAME = 'all jobs' or @SCHED_NAME = 'all jobs'
	BEGIN
		SELECT st.SCHED_NAME,
			jt.JOB_NAME,
			sjj.*
		FROM SCHEDULE st,
			SCHEDULE_JOB_JOIN sjj,
			JOB jt
		WHERE st.SCHED_RID = sjj.SCHED_RID
			AND jt.JOB_RID = sjj.JOB_RID
			AND
			(
				sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Running
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Queued
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Waiting
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Executed
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_OnHold
				)
		ORDER BY st.SCHED_RID
	END

	/*======================*/
	/* SCHEDNAME and JOBNAME*/
	/*======================*/
	ELSE IF @SCHED_NAME != '' AND @JOB_NAME != ''
	BEGIN
			SELECT st.SCHED_NAME,
			jt.JOB_NAME,
			sjj.*
		FROM SCHEDULE st,
			SCHEDULE_JOB_JOIN sjj,
			JOB jt
		WHERE st.SCHED_RID = sjj.SCHED_RID
			AND jt.JOB_RID = sjj.JOB_RID
			AND
			(
				sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Running
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Queued
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Waiting
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Executed
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_OnHold
				)
			AND st.SCHED_NAME = @SCHED_NAME
			AND jt.JOB_NAME like @JOB_NAME 
		ORDER BY st.SCHED_RID
	END

	/*====================*/
	/* USERRID and JOBNAME*/
	/*====================*/
	--ELSE IF @USER_RID != -1 AND @JOB_NAME != ''
	--BEGIN
	--		SELECT st.SCHED_NAME,
	--		jt.JOB_NAME,
	--		sjj.*
	--	FROM SCHEDULE st,
	--		SCHEDULE_JOB_JOIN sjj,
	--		JOB jt
	--	WHERE st.SCHED_RID = sjj.SCHED_RID
	--		AND jt.JOB_RID = sjj.JOB_RID
	--		AND
	--		(
	--			sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Running
	--			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Queued
	--			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Waiting
	--			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Executed
	--			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_OnHold
	--			)
	--		AND sjj.USER_RID = @USER_RID
	--		AND jt.JOB_NAME like @JOB_NAME 
	--	ORDER BY st.SCHED_RID
	--END

	/*===========*/
	/* USERNAME  */
	/*===========*/
	ELSE IF @USER_RID != -1
	BEGIN
			SELECT st.SCHED_NAME,
			jt.JOB_NAME,
			sjj.*
		FROM SCHEDULE st,
			SCHEDULE_JOB_JOIN sjj,
			JOB jt
		WHERE st.SCHED_RID = sjj.SCHED_RID
			AND jt.JOB_RID = sjj.JOB_RID
			AND
			(
				sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Running
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Queued
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Waiting
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Executed
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_OnHold
				)
			AND sjj.USER_RID = @USER_RID
		ORDER BY st.SCHED_RID
	END


	/*===============*/
	/* JOBNAME alone */
	/*===============*/
	ELSE IF @JOB_NAME != '' and @SCHED_NAME = ''
	BEGIN
			SELECT st.SCHED_NAME,
			jt.JOB_NAME,
			sjj.*
		FROM SCHEDULE st,
			SCHEDULE_JOB_JOIN sjj,
			JOB jt
		WHERE st.SCHED_RID = sjj.SCHED_RID
			AND jt.JOB_RID = sjj.JOB_RID
			AND
			(
				sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Running
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Queued
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Waiting
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Executed
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_OnHold
				)
			AND jt.JOB_NAME like @JOB_NAME
		ORDER BY st.SCHED_RID
	END

	/*=================*/
	/* SCHEDNAME alone */
	/*=================*/
	ELSE IF @SCHED_NAME != ''
	BEGIN
			SELECT st.SCHED_NAME,
			jt.JOB_NAME,
			sjj.*
		FROM SCHEDULE st,
			SCHEDULE_JOB_JOIN sjj,
			JOB jt
		WHERE st.SCHED_RID = sjj.SCHED_RID
			AND jt.JOB_RID = sjj.JOB_RID
			AND
			(
				sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Running
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Queued
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Waiting
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Executed
				OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_OnHold
				)
			AND st.SCHED_NAME like @SCHED_NAME
		ORDER BY st.SCHED_RID
	END

END
GO