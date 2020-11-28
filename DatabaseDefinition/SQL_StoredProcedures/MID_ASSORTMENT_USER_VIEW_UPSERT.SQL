--dv =============================================
--dv Author:		Steve Todd
--dv Create date:	12/02/2013
--dv Description:	Inserts/Updates a row in the ASSORTMENT_USER_VIEW_JOIN table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_USER_VIEW_UPSERT]
	@HDR_RID int, 
	@USER_RID int, 
	@VIEW_RID int                                                  
AS
BEGIN
	MERGE ASSORTMENT_USER_VIEW_JOIN T
	USING(SELECT 1 S) S
	ON T.HDR_RID = @HDR_RID and T.USER_RID = @USER_RID
	WHEN MATCHED THEN UPDATE SET 
	  VIEW_RID = @VIEW_RID  
	WHEN NOT MATCHED THEN
	  INSERT (HDR_RID, USER_RID, VIEW_RID)
	  VALUES(@HDR_RID, @USER_RID, @VIEW_RID);

	SELECT @@ROWCOUNT
END
GO