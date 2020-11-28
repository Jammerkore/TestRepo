--dv =============================================
--dv Create date:	5/26/2015
--dv Description:	Reads the RID of the first matching dynamic group
--dv				Case Insensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_READ_FOR_RENAME]
	@SG_ID VARCHAR(50),
	@USER_RID INT,
	@SCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 sg.SG_RID 
	FROM STORE_GROUP sg
	INNER JOIN FILTER f ON sg.FILTER_RID=f.FILTER_RID
	INNER JOIN FILTER_CONDITION fc ON f.FILTER_RID = fc.FILTER_RID
	WHERE sg.SG_DYNAMIC_GROUP_IND='1'
		AND sg.USER_RID = @USER_RID
		AND sg.IS_ACTIVE=1
		AND fc.ELEMENT_GROUP_TYPE_INDEX=44 
		AND fc.OPERATOR_INDEX = @SCG_RID


END
GO