--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Reads all store groups, sorted by rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_READ_ALL_SORTED_BY_RID]
	@LOAD_INACTIVE_GROUPS_IND CHAR  --  TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GlobalUserRID INT = 4;

        --  Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
	IF @LOAD_INACTIVE_GROUPS_IND = 1
	BEGIN
    	SELECT 
    		SG_RID, 
    		SG_ID, 
    		SG_DYNAMIC_GROUP_IND, 
    		coalesce(USER_RID, @GlobalUserRID) USER_RID,
    		coalesce(FILTER_RID, -2) FILTER_RID,
    		SG_VERSION
    	FROM STORE_GROUP
    	ORDER BY SG_RID ASC
	END
	ELSE
	BEGIN
	--  End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
    	SELECT 
		    SG_RID, 
		    SG_ID, 
		    SG_DYNAMIC_GROUP_IND, 
		    coalesce(USER_RID, @GlobalUserRID) USER_RID,
		    coalesce(FILTER_RID, -2) FILTER_RID,
		    SG_VERSION
	    FROM STORE_GROUP
	    WHERE IS_ACTIVE=1 
	    ORDER BY SG_RID ASC
	END  --  TT#1957-MD - JSmith - Purge fails deleting users with personal attributes

END
GO
