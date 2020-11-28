--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Returns the max store RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORES_READ_MAX]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		MAX(ST_RID) as MAX_STORE_RID 
    FROM STORES 
	
END
GO