--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads a size node rid based on the product hierarchy, parent color rid, and size id
--dv History: 07/30/2014 TT#1251-MD -jsobek -Error when adding/saving a Header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_NODE_READ_RID_FOR_COLOR]
	@PH_RID int,
	@COLOR_HN_RID int,
	@SIZE_CODE_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		sn.HN_RID  
	FROM HIER_NODE_JOIN hnj WITH (NOLOCK), 
	     SIZE_NODE sn WITH (NOLOCK), 
		 SIZE_CODE sc WITH (NOLOCK) 
	WHERE hnj.PH_RID = @PH_RID
		  AND hnj.PARENT_HN_RID = @COLOR_HN_RID
		  AND sn.HN_RID = hnj.HN_RID
		  AND sc.SIZE_CODE_ID = @SIZE_CODE_ID
		  AND sn.SIZE_CODE_RID = sc.SIZE_CODE_RID

END
GO
