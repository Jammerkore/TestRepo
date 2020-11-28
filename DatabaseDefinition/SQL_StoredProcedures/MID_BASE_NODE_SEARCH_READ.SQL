--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Searches base nodes on a variety of criteria, and if found, returns the matching hierarchy rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BASE_NODE_SEARCH_READ]
	@BN_SEARCH_STRING varchar(356)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_SEARCH_STRING] (@BN_SEARCH_STRING) AS HN_RID

END
GO
