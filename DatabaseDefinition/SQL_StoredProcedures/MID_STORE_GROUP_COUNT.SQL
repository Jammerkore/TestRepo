--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Reads all store groups, sorted by rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_COUNT]
	@LOAD_INACTIVE_GROUPS_IND CHAR 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GlobalUserRID INT = 4;

	IF @LOAD_INACTIVE_GROUPS_IND = 1
	BEGIN
    	SELECT 
    		COUNT(*) MyCount 
    	FROM STORE_GROUP
	END
	ELSE
	BEGIN
    	SELECT 
		    COUNT(*) MyCount 
	    FROM STORE_GROUP
	    WHERE IS_ACTIVE=1 
	END  

END
GO
