-- ================================================
--
-- Author:		Geoffrey Taylor
-- Create date: 1/20/2012
-- Description:	this stored procedure will check
--				for any locked ancestors
--
-- ================================================
CREATE PROCEDURE [dbo].[SP_MID_ANY_ANCESTORS_LOCKED] 
	@HN_RID INT
	WITH RECOMPILE
AS
SET NOCOUNT ON

BEGIN
	DECLARE @PH_RID INT
	DECLARE @TOTALHNRID INT

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	SET @TOTALHNRID = 0

	-- check if there are any branch locks.  If not, no reason to check ancestors
	IF EXISTS (
			SELECT ENQUEUE_TYPE
			FROM MID_ENQUEUE
			WHERE ENQUEUE_TYPE = 10
			)
	BEGIN
		SELECT @PH_RID = HOME_PH_RID
		FROM [dbo].[HIERARCHY_NODE]
		WHERE HN_RID = @HN_RID

		-- define temp table for tracking ancestors	
		DECLARE @ANCESTORS TABLE (
			ANCESTOR_RID INT,
			HOME_PH_RID INT,
			HOME_LEVEL INT
			)

		INSERT INTO @ANCESTORS
		EXEC SP_MID_GET_ANCESTORS @HN_RID,
			@PH_RID,
			0,
			0

		-- select ancestors with a branch lock
		SELECT me.USER_RID,
			me.HN_RID,
			CASE au.USER_FULLNAME
				WHEN ''
					THEN au.USER_NAME
				ELSE au.USER_FULLNAME
				END AS USER_FULLNAME
		FROM [dbo].[MID_ENQUEUE] me
		JOIN [dbo].[APPLICATION_USER] au ON au.USER_RID = me.USER_RID
		WHERE ENQUEUE_TYPE IN (10)
			AND HN_RID IN (
				SELECT ANCESTOR_RID
				FROM @ANCESTORS
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


