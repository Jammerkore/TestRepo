--dv =============================================
--dv Create date:	5/26/2015
--dv Description:	Returns a count of rows that match the group name, user, and store char group
--dv				Case Insensitive
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_READ_FOR_DUPLICATE_FOR_STORE_CHAR_GROUP]
	@SG_ID VARCHAR(50),
	@USER_RID INT,
	@SCG_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(sg2.SG_RID) AS MYCOUNT
	FROM STORE_GROUP sg2
	WHERE sg2.SG_RID NOT IN 
	(
		SELECT TOP 1 sg.SG_RID --only the first matching dynamic group will have its name changed
		FROM STORE_GROUP sg
		INNER JOIN FILTER f ON sg.FILTER_RID=f.FILTER_RID
		INNER JOIN FILTER_CONDITION fc ON f.FILTER_RID = fc.FILTER_RID
		WHERE  UPPER(sg.SG_ID)=UPPER(@SG_ID)
		    AND sg.SG_DYNAMIC_GROUP_IND='1'
			AND sg.USER_RID = @USER_RID
			AND sg.IS_ACTIVE=1
			AND fc.ELEMENT_GROUP_TYPE_INDEX=44 
			AND fc.OPERATOR_INDEX = @SCG_RID
	)
	AND sg2.SG_RID IN 
	(
		SELECT DISTINCT sg.SG_RID
		FROM STORE_GROUP sg
		INNER JOIN FILTER f ON sg.FILTER_RID=f.FILTER_RID
		INNER JOIN FILTER_CONDITION fc ON f.FILTER_RID = fc.FILTER_RID
		WHERE UPPER(SG_ID)=UPPER(@SG_ID) 
			--AND sg.SG_DYNAMIC_GROUP_IND='1'
			AND sg.USER_RID = @USER_RID
			AND sg.IS_ACTIVE=1
			--AND fc.ELEMENT_GROUP_TYPE_INDEX=44 
	)


END
GO