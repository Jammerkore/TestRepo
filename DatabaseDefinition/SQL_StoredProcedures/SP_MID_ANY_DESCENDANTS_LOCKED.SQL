CREATE PROCEDURE [dbo].[SP_MID_ANY_DESCENDANTS_LOCKED] 
	@HN_RID INT
	WITH RECOMPILE
AS
SET NOCOUNT ON

BEGIN
	DECLARE @TOTALHNRID INT

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

	-- check if there are any node or branch locks.  If not, no reason to check ancestors
	IF EXISTS (
			SELECT ENQUEUE_TYPE
			FROM MID_ENQUEUE
			WHERE ENQUEUE_TYPE IN (
					6,
					10
					)
			)
	BEGIN
		-- define temp table for tracking descendants	
		DECLARE @DESCENDANTS TABLE (
			HOME_PH_RID INT,
			HN_RID INT NOT NULL,
			HOME_LEVEL INT NOT NULL,
			PARENT_HN_RID INT
			)

		-- insert node into table
		--INSERT INTO @DESCENDANTS
		--  select HOME_PH_RID, PH_TYPE, HN_RID, HOME_LEVEL, 0
		--    from [dbo].[HIERARCHY_NODE] hn
		--    JOIN [dbo].[PRODUCT_HIERARCHY] ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
		--   where HN_RID = @HN_RID 	
		INSERT INTO @DESCENDANTS
		EXEC SP_MID_GET_ALL_DESCENDANTS @HN_RID

		SELECT me.USER_RID,
			me.HN_RID,
			CASE au.USER_FULLNAME
				WHEN ''
					THEN au.USER_NAME
				ELSE au.USER_FULLNAME
				END AS USER_FULLNAME
		FROM [dbo].[MID_ENQUEUE] me
		JOIN [dbo].[APPLICATION_USER] au ON au.USER_RID = me.USER_RID
		WHERE ENQUEUE_TYPE IN (
				6,
				10
				)
			AND HN_RID IN (
				SELECT HN_RID
				FROM @DESCENDANTS
				)

		SET @TOTALHNRID = @@ROWCOUNT
	END

	-- are there any ancestors locked?
	-- return true/false
	IF (@TOTALHNRID > 0)
		RETURN 1
	ELSE
		RETURN 0
END
GO


