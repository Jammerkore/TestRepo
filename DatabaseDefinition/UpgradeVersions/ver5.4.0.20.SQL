/*RELEASE 5.4.0.20 11-12-2014 -TT#1347-MD - STodd - System Deny */
BEGIN
	-- USERS
  	WITH cte_users(USER_RID, USER_ACTIVE_IND)
	AS (
		SELECT
		au.USER_RID, au.USER_ACTIVE_IND
		FROM 
		[dbo].[APPLICATION_USER] au
		where au.USER_RID > 4 
		and au.USER_RID not in (select USER_RID from dbo.SECURITY_USER_FUNCTION where FUNC_ID = 810050) -- "System" record doesn't areay exist
		)

	-- "System" record doesn't exist and user is NOT in a group
	INSERT into dbo.SECURITY_USER_FUNCTION (USER_RID, FUNC_ID, SEC_LVL_ID, ACTION_ID)
	SELECT cte.USER_RID, 810050, 801005, 801160
	FROM cte_users cte 
	where USER_RID not in (select USER_RID from dbo.USER_GROUP_JOIN)	-- Not in ANY group 
	order by USER_RID

	-- Administration or Globals Options DOES exist and User is in a group and user "System" record does not exist
	INSERT into dbo.SECURITY_USER_FUNCTION (USER_RID, FUNC_ID, SEC_LVL_ID, ACTION_ID)
	select Distinct suf.USER_RID, 810050, 801005, 801160 from dbo.SECURITY_USER_FUNCTION suf, USER_GROUP_JOIN ugj 
	where (suf.FUNC_ID = 810000 or suf.FUNC_ID = 810400) and ugj.USER_RID <> suf.USER_RID
	and suf.USER_RID not in (select USER_RID from dbo.SECURITY_USER_FUNCTION where FUNC_ID = 810050)

	-- GROUPS
	-- All non-administrator groups
	INSERT into dbo.SECURITY_GROUP_FUNCTION (GROUP_RID, FUNC_ID, SEC_LVL_ID, ACTION_ID)
	SELECT DISTINCT GROUP_RID, 810050, 801005, 801160
	FROM dbo.SECURITY_GROUP_FUNCTION 
	where GROUP_RID <> 1
END	
GO

