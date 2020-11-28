--dv =============================================
--dv Create date:	4/28/2015
--dv Description:	Clears version history for store group joins and store group level results, as well as inactive store group levels and inactive store groups
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_JOIN_HISTORY_DELETE_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	TRUNCATE TABLE STORE_GROUP_JOIN_HISTORY
	TRUNCATE TABLE STORE_GROUP_LEVEL_RESULTS_HISTORY

	-- DELETE old versions
	DECLARE @maxVersion TABLE (RID int, VERSION int)

	-- DELETE old attribute information
	INSERT INTO @maxVersion
	SELECT sg.SG_RID, max(sg.SG_VERSION)
	FROM STORE_GROUP sg 
	GROUP BY sg.SG_RID

	DELETE sgj
	FROM STORE_GROUP_JOIN sgj WITH (ROWLOCK)
	JOIN @maxVersion mv ON mv.RID = sgj.SG_RID
	WHERE sgj.SG_VERSION < mv.VERSION

	-- DELETE old set information
	-- get max version of all sets
	DELETE FROM @maxVersion

	INSERT INTO @maxVersion
	SELECT sgl.SGL_RID, max(sgl.SGL_VERSION)
	FROM STORE_GROUP_LEVEL sgl 
	GROUP BY sgl.SGL_RID

	DELETE sgj
	FROM STORE_GROUP_JOIN sgj WITH (ROWLOCK)
	JOIN @maxVersion mv ON mv.RID = sgj.SGL_RID
	WHERE sgj.SGL_VERSION < mv.VERSION

	DELETE sglr
	FROM STORE_GROUP_LEVEL_RESULTS sglr WITH (ROWLOCK)
	JOIN @maxVersion mv ON mv.RID = sglr.SGL_RID
	WHERE sglr.SGL_VERSION < mv.VERSION
	
        -- Delete inactive store groups and levels

	-- Begin RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets
	-- Delete join entries for inactive store group levels
	DELETE FROM STORE_GROUP_JOIN WITH (ROWLOCK)
	WHERE STORE_GROUP_JOIN.SGL_RID IN (SELECT SGL_RID FROM STORE_GROUP_LEVEL WHERE IS_ACTIVE=0)
	-- End RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets

	DELETE FROM STORE_GROUP_LEVEL WITH (ROWLOCK) WHERE IS_ACTIVE=0

	--Delete user items for inactive store groups
	DELETE 
	FROM USER_ITEM WITH (ROWLOCK)
	WHERE ITEM_TYPE = 38 --Store Group 
		  AND ITEM_RID IN (SELECT SG_RID FROM STORE_GROUP WHERE IS_ACTIVE=0)

	--Delete levels for inactive store groups
	DELETE FROM STORE_GROUP_LEVEL_RESULTS WITH (ROWLOCK)
	WHERE STORE_GROUP_LEVEL_RESULTS.SGL_RID IN (SELECT SGL_RID FROM STORE_GROUP_LEVEL sgl WHERE sgl.SG_RID IN (SELECT SG_RID FROM STORE_GROUP WHERE IS_ACTIVE=0))

	--Delete join entries for inactive store groups
	DELETE FROM STORE_GROUP_JOIN WITH (ROWLOCK)
	WHERE STORE_GROUP_JOIN.SG_RID IN (SELECT SG_RID FROM STORE_GROUP WHERE IS_ACTIVE=0)

	DELETE FROM STORE_GROUP_LEVEL WITH (ROWLOCK) WHERE SG_RID in (SELECT SG_RID FROM STORE_GROUP WHERE IS_ACTIVE=0)

	--Delete the filters for inactive store groups
	DELETE FROM FILTER WITH (ROWLOCK)
	WHERE FILTER_RID IN 
	(
		SELECT FILTER_RID FROM STORE_GROUP WITH (ROWLOCK)
		WHERE IS_ACTIVE=0
	)

	--Delete inactive store groups
	DELETE FROM STORE_GROUP WITH (ROWLOCK)
	WHERE IS_ACTIVE=0

	SELECT @@ROWCOUNT;
END
GO
