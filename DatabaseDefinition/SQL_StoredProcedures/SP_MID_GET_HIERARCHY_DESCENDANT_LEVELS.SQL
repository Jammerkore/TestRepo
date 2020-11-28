--dv Create date:	9/15/2015
--dv Modified:		9/15/2015
--dv Description:	Retrieves descendant node levels from a starting node
--dv                TT#1721-MD - RMatelic - Data Layer Request - Add CTE and Stored Procedure that will return the levels under an alternate hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_GET_HIERARCHY_DESCENDANT_LEVELS] 
@HN_RID INT

AS 
BEGIN 

SELECT * 
FROM [dbo].[UDF_HIERARCHY_GET_ALL_DESCENDANT_LEVELS] (@HN_RID)

END

GO


