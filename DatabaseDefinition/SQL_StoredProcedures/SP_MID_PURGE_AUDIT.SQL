CREATE PROCEDURE [dbo].[SP_MID_PURGE_AUDIT] 
	@PURGE_DAYS INT,
	@COMMIT_LIMIT INT,
	@RECORDS_DELETED INT OUTPUT
AS
BEGIN
	DECLARE @purge_date DATETIME

	SET @purge_date = getdate() - @PURGE_DAYS
	SET @RECORDS_DELETED = 0

	DECLARE @t TABLE (PROCESS_RID INT)

	INSERT INTO @t
	SELECT DISTINCT PROCESS_RID
	FROM PROC_HDR
	WHERE START_TIME < @purge_date
		AND EXECUTION_STATUS_CODE != 801602 -- Running -- TT#3477 - JSmith - Single Style Size Curve Error

	IF @@rowcount = 0
		RETURN

	-- delete hierarchy load information
	DELETE TOP (@COMMIT_LIMIT)
	FROM HIER_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete posting information
	DELETE TOP (@COMMIT_LIMIT)
	FROM POSTING_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete store load information
	DELETE TOP (@COMMIT_LIMIT)
	FROM STR_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete purge information
	DELETE TOP (@COMMIT_LIMIT)
	FROM PURGE_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete header load information
	DELETE TOP (@COMMIT_LIMIT)
	FROM HEADER_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete build pack criteria load information
	--Begin TT#808 -- Create separate upgrade script for Stored Procedure
	--delete from BUILD_PACK_CRITERIA_LOAD_INFO
	DELETE TOP (@COMMIT_LIMIT)
	FROM BUILD_PACK_CRITERIA_LOAD_INFO
	--End TT#808 -- Create separate upgrade script for Stored Procedure
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete color code load information
	DELETE TOP (@COMMIT_LIMIT)
	FROM COLOR_CODE_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete size code load information
	DELETE TOP (@COMMIT_LIMIT)
	FROM SIZE_CODE_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete rollup process information
	DELETE TOP (@COMMIT_LIMIT)
	FROM ROLLUP_PROCESS
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	DELETE TOP (@COMMIT_LIMIT)
	FROM ROLLUP_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete reclass audit information
	DELETE TOP (@COMMIT_LIMIT)
	FROM AUDIT_RECLASS
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete reclass rejected header information
	DELETE TOP (@COMMIT_LIMIT)
	FROM RECLASS_REJECTED_HEADER
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete computation process information
	DELETE TOP (@COMMIT_LIMIT)
	FROM COMPUTATION_PROCESS
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete computation driver information
	DELETE TOP (@COMMIT_LIMIT)
	FROM COMPUTATION_DRIVER_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete size constraint load information
	DELETE TOP (@COMMIT_LIMIT)
	FROM SIZE_CONSTRAINT_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete size curve load information
	DELETE TOP (@COMMIT_LIMIT)
	FROM SIZE_CURVE_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete relieve intransit information
	DELETE TOP (@COMMIT_LIMIT)
	FROM AUDIT_RELIEVE_INTRANSIT_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete special request information
	DELETE TOP (@COMMIT_LIMIT)
	FROM SPECIAL_REQUEST_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete size curve generate information
	DELETE TOP (@COMMIT_LIMIT)
	FROM SIZE_CURVE_GENERATE_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* Begin TT#781 - JScott - Purge fails. Build Packs constraint & Plan View User constraint */
	-- delete build pack criteria load information
	DELETE TOP (@COMMIT_LIMIT)
	FROM BUILD_PACK_CRITERIA_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* End TT#781 - JScott - Purge fails. Build Packs constraint & Plan View User constraint */
	/* Begin TT#1011 ï¿½ Jsmith - Purge failed with DB error */
	-- delete build pack criteria load information
	DELETE
	FROM SIZE_CURVE_GENERATE_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* End TT#1011 */
	/* Begin TT#1035 ï¿½ Jsmith - Purge failed with DB error */
	-- delete build pack criteria load information
	DELETE
	FROM GENERATE_RELIEVE_INTRANSIT_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* End TT#1035 */
	/* Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model */
	-- delete node activity load information
	DELETE TOP (@COMMIT_LIMIT)
	FROM DETERMINE_NODE_ACTIVITY_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* End TT#988 */
	/*Begin TT#2445 - RBeck -  Purge Error - Push to Back Stock */
	DELETE TOP (@COMMIT_LIMIT)
	FROM PUSH_TO_BACK_STOCK_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* End TT#2445 */
	/*Begin TT#536-MD - JSmith -  Purge Error - Database conflict error when running Purge process */
	DELETE TOP (@COMMIT_LIMIT)
	FROM DAILY_PERCENTAGES_CRITERIA_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* End TT#536-MD - JSmith -  Purge Error - Database conflict error when running Purge process */
	-- delete audit detail
	DELETE TOP (@COMMIT_LIMIT)
	FROM PROC_RPT
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* Begin TT#228 - RBeck - Hierarchy Reclass message incorrectly displayed */
	DELETE TOP (@COMMIT_LIMIT)
	FROM HIER_RECLASS_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* End TT#228 */
	/* Begin TT#3465-M-VStuart-Size Day to Week Summary Audit not Purging correctly-MID */
	-- delete Size Day to Week Summary Audit information
	DELETE TOP (@COMMIT_LIMIT)
	FROM SIZE_DAY_TO_WEEK_SUMMARY_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete VSW Criteria Load Audit information
	DELETE TOP (@COMMIT_LIMIT)
	FROM VSW_CRITERIA_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* End TT#3465-M-VStuart-Size Day to Week Summary Audit not Purging correctly-MID */
	/* Begin TT#3481 - JSmith - Tables missing from audit purge */
	-- delete Chain Set Percent Criteria Load Summary Audit information
	DELETE TOP (@COMMIT_LIMIT)
	FROM CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	-- delete Store Eligibility Criteria Load Audit information
	DELETE TOP (@COMMIT_LIMIT)
	FROM STORE_ELIGIBILITY_CRITERIA_LOAD_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount

	/* End TT#3481 - JSmith - Tables missing from audit purge */

	/* BEGIN TT#4643-VStuart-Foreign Key reference when purging the audit for Header Reconcile-MID */
	-- delete Header Reconcile information
	DELETE TOP (@COMMIT_LIMIT)
	FROM HEADER_RECONCILE_INFO
	WHERE PROCESS_RID IN (
			SELECT PROCESS_RID
			FROM @t
			)

	SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount
	/* END TT#4643-VStuart-Foreign Key reference when purging the audit for Header Reconcile-MID */

	-- delete audit headers after all other records are deleted
	IF @RECORDS_DELETED = 0
	BEGIN
		DELETE TOP (@COMMIT_LIMIT)
		FROM PROC_HDR
		WHERE PROCESS_RID IN (
				SELECT PROCESS_RID
				FROM @t
				)

		SET @RECORDS_DELETED = @RECORDS_DELETED + @@rowcount
	END
END;
GO


