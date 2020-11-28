CREATE PROCEDURE [dbo].[SP_MID_GET_NODE_RIDS] 
	(
	@xmlDoc NTEXT,
	@debug BIT = 0
	)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @idoc INT

	EXEC sp_xml_preparedocument @idoc OUTPUT,
		@xmlDoc

	CREATE TABLE #NODE_IDS (NODE_ID VARCHAR(50) COLLATE database_default NOT NULL)

	CREATE INDEX #NODE_IDS_IDX_1 ON #NODE_IDS (NODE_ID)

	-- Process records
	INSERT #NODE_IDS (NODE_ID)
	SELECT xmlID
	FROM OPENXML(@idoc, '/root/node', 2) WITH (xmlID VARCHAR(250) '@ID') xmlnodes

	SELECT node.NODE_ID,
		bn.HN_RID
	FROM BASE_NODE bn,
		#NODE_IDS node
	WHERE node.NODE_ID = bn.BN_ID

	SET NOCOUNT OFF
END
GO


