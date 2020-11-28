--dv =============================================
--dv Create date:	10/15/2014
--dv Description:	Retrieves header columns for the allocate task tasklist for the header RIDs in the list
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HEADER_READ_FOR_TASKLIST] 
(	
	@HDR_RID_LIST HDR_RID_TYPE READONLY
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		HDR_RID, 
		HDR_ID 
	FROM HEADER h WITH (NOLOCK)
	WHERE h.HDR_RID IN (SELECT HDR_RID FROM @HDR_RID_LIST)
)
GO