--dv Create date:	9/15/2013
--dv Description:	Returns the first merchandise hierarchy node rid (HN_RID) from a base node description  
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_DESCRIPTION]
(
	@BN_DESCRIPTION varchar(250)
)
RETURNS INT
AS
BEGIN
	DECLARE @HN_RID INT
	
	SET @BN_DESCRIPTION = RTRIM(@BN_DESCRIPTION)

	SET @HN_RID = 
	(
		SELECT TOP 1 hn.HN_RID 
					FROM HIERARCHY_NODE hn (NOLOCK) 
					INNER JOIN BASE_NODE bn (NOLOCK) ON bn.HN_RID=hn.HN_RID
					WHERE RTRIM(bn.BN_DESCRIPTION) = @BN_DESCRIPTION
	)

	RETURN @HN_RID

END
GO
