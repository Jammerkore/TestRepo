--dv Create date:	9/15/2013
--dv Description:	Returns the first merchandise hierarchy node rid (HN_RID) from a base node text 
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_TEXT]
(
	@BN_TEXT varchar(356)
)
RETURNS INT
AS
BEGIN
	DECLARE @HN_RID INT
	

	SET @BN_TEXT = RTRIM(@BN_TEXT)

	SET @HN_RID = 
	(
		SELECT TOP 1 HN_RID FROM 
		(
		SELECT hn.HN_RID,
		(SELECT [dbo].[UDF_HIERARCHY_BUILD_BASE_NODE_DISPLAY_ID](phl.PHL_DISPLAY_OPTION_ID, bn.BN_ID, bn.BN_NAME, bn.BN_DESCRIPTION) ) AS BASE_TEXT
		FROM
		HIERARCHY_NODE hn (NOLOCK) 
		INNER JOIN BASE_NODE bn (NOLOCK) ON bn.HN_RID=hn.HN_RID
		INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl (NOLOCK) on phl.PHL_SEQUENCE=hn.HOME_LEVEL
		) AS T
		WHERE @BN_TEXT=BASE_TEXT				
	)

	RETURN @HN_RID

END
GO
