--dv =============================================
--dv Create date: 1/4/2016
--dv Description: Loads STORE_GROUP_JOIN and STORE_GROUP_JOIN_HISTORY for all group levels.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_JOIN_FIRST_TIME_INIT]
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM STORE_GROUP_JOIN)
	BEGIN
        INSERT INTO STORE_GROUP_JOIN (SG_RID,SG_VERSION,SGL_RID,SGL_VERSION,STORE_COUNT,SGL_OVERRIDE_ID,SGL_OVERRIDE_SEQUENCE) 
        SELECT
        SG_RID,
        1,
        sgl.SGL_RID,
        1,
        0,
        sgl.SGL_ID,
        sgl.SGL_SEQUENCE
        FROM STORE_GROUP_LEVEL sgl

		INSERT INTO STORE_GROUP_JOIN_HISTORY (SG_RID,SG_VERSION,SGL_RID,SGL_VERSION,STORE_COUNT,SGL_OVERRIDE_ID,SGL_OVERRIDE_SEQUENCE) 
        SELECT
        SG_RID,
        1,
        sgl.SGL_RID,
        1,
        0,
        sgl.SGL_ID,
        sgl.SGL_SEQUENCE
        FROM STORE_GROUP_LEVEL sgl
	END
END
GO