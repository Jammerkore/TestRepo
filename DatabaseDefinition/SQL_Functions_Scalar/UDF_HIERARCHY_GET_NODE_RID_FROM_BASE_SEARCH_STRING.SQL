--dv Create date: 9/15/2013
--dv Description:	Searches base nodes to attempt to find the first match based on a search string
--dv				Searches for an exact match (trimmed) on base id first
--dv				Then searches for an exact match (trimmed) on the base name
--dv				Then searches for an exact match (trimmed) on the base description
--dv				Finally, searches for the entire base node level text that shows in the merchandies hierarchy
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_SEARCH_STRING]
(
	@BN_SEARCH_STRING VARCHAR(356)
)
RETURNS INT
AS
BEGIN
	DECLARE @HN_RID INT

	DECLARE @BN_SEARCH_ID VARCHAR(50)
	DECLARE @BN_SEARCH_NAME VARCHAR(50)
	DECLARE @BN_SEARCH_DESCRIPTION VARCHAR(250)

	IF (LEN(@BN_SEARCH_STRING) > 50)
		SET @BN_SEARCH_ID = SUBSTRING(@BN_SEARCH_STRING, 1, 50)
	ElSE
		SET @BN_SEARCH_ID = @BN_SEARCH_STRING

	IF (LEN(@BN_SEARCH_STRING) > 50)
		SET @BN_SEARCH_NAME = SUBSTRING(@BN_SEARCH_STRING, 1, 50)
	ElSE
		SET @BN_SEARCH_NAME = @BN_SEARCH_STRING

	IF (LEN(@BN_SEARCH_STRING) > 250)
		SET @BN_SEARCH_DESCRIPTION = SUBSTRING(@BN_SEARCH_STRING, 1, 250)
	ElSE
		SET @BN_SEARCH_DESCRIPTION = @BN_SEARCH_STRING

	

	--Search base nodes on ID first
	SET @HN_RID = (SELECT [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_ID] (@BN_SEARCH_ID))
	IF (@HN_RID IS NULL)
	BEGIN
		--Then search base nodes on NAME
		SET @HN_RID = (SELECT [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_NAME] (@BN_SEARCH_NAME))
		IF (@HN_RID IS NULL)
		BEGIN
			--Then search base nodes on DESCRIPTION
			SET @HN_RID = (SELECT [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_DESCRIPTION] (@BN_SEARCH_DESCRIPTION))
			IF (@HN_RID IS NULL)
			BEGIN
				--Then search base nodes on TEXT
				SET @BN_SEARCH_STRING = RTRIM(@BN_SEARCH_STRING)
				SET @HN_RID = (SELECT [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_TEXT] (@BN_SEARCH_STRING))
			END
		END
	END

	RETURN @HN_RID

END
GO
