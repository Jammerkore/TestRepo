--dv =============================================
--dv Create date:	5/15/2015
--dv Description:	Inserts a specific All Stores attribute 
--dv History:		5/15/2015 -TT#1517-MD -jsobek -Store Service Optimization
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_INSERT_ALL_STORES_SET]
	@FILTER_RID INT 
AS
BEGIN
	SET NOCOUNT ON;

	/*  ALL STORE STORE GROUP  */
	SET IDENTITY_INSERT STORE_GROUP ON
	/* begin TT # 92 stodd */
	INSERT INTO STORE_GROUP(SG_RID, SG_ID, SG_DYNAMIC_GROUP_IND, USER_RID, FILTER_RID, IS_ACTIVE, SG_VERSION)
	VALUES(1,'All Stores', '0', 4, @FILTER_RID, 1, 1);
	/* End TT # 92 stodd */
	SET IDENTITY_INSERT STORE_GROUP OFF
	/* seed the STORE_GROUP table that will allow for future static record ids */
	DBCC CHECKIDENT (STORE_GROUP,reseed,100)

	
	SET IDENTITY_INSERT STORE_GROUP_LEVEL ON
	INSERT INTO STORE_GROUP_LEVEL(SGL_RID, SGL_SEQUENCE, SG_RID, SGL_ID, IS_ACTIVE, SGL_VERSION, LEVEL_TYPE)
	VALUES (1,2147483647,1,'All Stores Set', 1, 1, 1);
	SET IDENTITY_INSERT STORE_GROUP_LEVEL OFF
	/* seed the STORE_GROUP_LEVEL table that will allow for future static record ids */
	DBCC CHECKIDENT (STORE_GROUP_LEVEL,reseed,100)


	INSERT INTO STORE_GROUP_JOIN(SG_RID, SG_VERSION, SGL_RID, SGL_VERSION, STORE_COUNT, SGL_OVERRIDE_ID, SGL_OVERRIDE_SEQUENCE)
	VALUES (1, 1, 1, 1, 0, 'All Stores Set', 2147483647)

	SELECT 1;

END
GO