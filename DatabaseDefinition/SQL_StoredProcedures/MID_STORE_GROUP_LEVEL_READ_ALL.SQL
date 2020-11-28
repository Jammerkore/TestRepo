--dv =============================================
--dv Create date:	12/12/2013
--dv Description:	Get all store group level info.
--dv History:		4/15/2015 TT#1517-MD -jsobek -Store Service Optimization
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_LEVEL_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	--SELECT *
	--FROM STORE_GROUP_LEVEL
	--ORDER BY SG_RID,
	--	SGL_SEQUENCE


	SELECT 
		sgj.SGL_RID, 
		sgj.SGL_OVERRIDE_SEQUENCE AS SGL_SEQUENCE,
		sgj.SG_RID, 
		sgj.SGL_OVERRIDE_ID AS SGL_ID,
		sgj.SGL_VERSION,
		sgl.LEVEL_TYPE   
	FROM 
	STORE_GROUP sg WITH (NOLOCK) 
	INNER JOIN STORE_GROUP_LEVEL sgl WITH (NOLOCK) ON sgl.SG_RID=sg.SG_RID 
	INNER JOIN STORE_GROUP_JOIN sgj WITH (NOLOCK) ON sgl.SGL_RID=sgj.SGL_RID 
	WHERE sgj.SG_RID=sg.SG_RID AND sgj.SG_VERSION=sg.SG_VERSION AND sgj.SGL_VERSION=sgl.SGL_VERSION
	ORDER BY sgl.SG_RID,
		sgj.SGL_OVERRIDE_SEQUENCE

END
GO