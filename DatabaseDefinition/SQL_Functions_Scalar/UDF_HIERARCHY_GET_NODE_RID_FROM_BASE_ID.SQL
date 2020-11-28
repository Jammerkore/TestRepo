--dv Create date:	5/8/2013
--dv Description:	Returns the first merchandise hierarchy node rid (HN_RID) from a base node id  
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_ID]
(
	@BN_ID varchar(50)
)
RETURNS INT
AS
BEGIN
	DECLARE @HN_RID INT
	SET @BN_ID = RTRIM(@BN_ID)

	SET @HN_RID = 
	(
		SELECT TOP 1 hn.HN_RID 
					FROM HIERARCHY_NODE hn (NOLOCK) 
					INNER JOIN BASE_NODE bn (NOLOCK) ON bn.HN_RID=hn.HN_RID
					WHERE RTRIM(bn.BN_ID) = @BN_ID
	)

	RETURN @HN_RID

END
GO
