--dv ============================================= 
--dv Create date: 5/7/2013
--dv Description:	Gets a list of active store rids
--dv History:	5/23/2014 TT#3735 -jsobek -Size Day To Week Summary - Add NOLOCK 
--dv =============================================
CREATE FUNCTION [dbo].[UDF_STORE_GET_ACTIVE_RIDS]
(	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ST_RID
	FROM [dbo].[STORES] st WITH (NOLOCK)  --TT#3735 -jsobek -Size Day To Week Summary - Add NOLOCK
	  WHERE st.ACTIVE_IND=1 
)

GO


