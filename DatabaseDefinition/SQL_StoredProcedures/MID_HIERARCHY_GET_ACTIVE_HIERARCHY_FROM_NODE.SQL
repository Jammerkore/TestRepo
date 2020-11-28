--dv =============================================
--dv Create date: 5/15/2013
--dv Description:	Retrieves active descendant and ancestor node data from a starting node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_GET_ACTIVE_HIERARCHY_FROM_NODE]
 @SELECTED_NODE_RID  INT
 AS
 BEGIN

	SELECT * FROM
	(
		SELECT * FROM [dbo].[UDF_HIERARCHY_GET_ACTIVE_ANCESTOR_NODES] (@SELECTED_NODE_RID)	   
		UNION
		SELECT * FROM [dbo].[UDF_HIERARCHY_GET_ACTIVE_DESCENDANT_NODES] (@SELECTED_NODE_RID)
	) AS Q1
	ORDER BY HOME_LEVEL

 END
GO
