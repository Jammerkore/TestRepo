CREATE PROCEDURE [dbo].[SP_MID_HIERARCHY_IN_USE_BY_HEADER] 
	@PH_RID INT,
	@ReturnCode INT OUTPUT
AS
BEGIN
	SET @ReturnCode = 0

	/* Procedure checks to see if any nodes in the hierarchy are used in header tables */
	CREATE TABLE #t (RID INT)

	CREATE CLUSTERED INDEX t_idx ON #t (RID)

	-- get all nodes in hierarchy
	INSERT INTO #t
	SELECT HN_RID
	FROM HIERARCHY_NODE
	WHERE HOME_PH_RID = @PH_RID

	/* if so,  we can not delete the size curve group                                  */
	DECLARE @rowsAffected INT

	SELECT TOP 1 PLAN_HNRID
	FROM HEADER
	WHERE PLAN_HNRID IN (
			SELECT RID
			FROM #t
			)

	SELECT @rowsAffected = @@rowcount

	IF @rowsAffected > 0
		SET @ReturnCode = 1

	IF @ReturnCode = 0
	BEGIN
		SELECT TOP 1 ON_HAND_HNRID
		FROM HEADER
		WHERE ON_HAND_HNRID IN (
				SELECT RID
				FROM #t
				)

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END

	--//BEGIN TT#5236-VStuart-PO purge error during User Purge-BonTon
	IF @ReturnCode = 0
	BEGIN
		SELECT TOP 1 GRADE_INVENTORY_HNRID
		FROM HEADER
		WHERE GRADE_INVENTORY_HNRID IN (
				SELECT RID
				FROM #t
				)

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END
	--//END TT#5236-VStuart-PO purge error during User Purge-BonTon

	IF @ReturnCode = 0
	BEGIN
		SELECT TOP 1 HN_RID
		FROM HEADER_SIZE_NEED
		WHERE HN_RID IN (
				SELECT RID
				FROM #t
				)

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END

	IF @ReturnCode = 0
	BEGIN
		SELECT TOP 1 HN_RID
		FROM HEADER_BULK_COLOR_SIZE_NEED
		WHERE HN_RID IN (
				SELECT RID
				FROM #t
				)

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END

	IF @ReturnCode = 0
	BEGIN
		-- Begin TT#3238 - JSmith - Purge Failed in MIDPRod
		--select TOP 1 HN_RID from HEADER_ASSORTMENT_BASIS where HN_RID in (select RID from #t)
		SELECT TOP 1 HN_RID
		FROM ASSORTMENT_PROPERTIES_BASIS
		WHERE HN_RID IN (
				SELECT RID
				FROM #t
				)

		-- End TT#3238 - JSmith - Purge Failed in MIDPRod
		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END

	IF (
			SELECT object_id('tempdb.dbo.#t')
			) > 0
		DROP TABLE #t
END
GO


