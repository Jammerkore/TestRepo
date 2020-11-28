--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns enqueue data based on the type and RID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_READ_FROM_TYPE]
	@RID int,
	@ENQUEUE_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		me.USER_RID, 
		me.RID, 
        case au.USER_FULLNAME when '' then au.USER_NAME else au.USER_FULLNAME end as USER_FULLNAME 
    FROM MID_ENQUEUE me, 
		 APPLICATION_USER au 
	WHERE ENQUEUE_TYPE=@ENQUEUE_TYPE
		 AND RID = @RID
		 AND me.USER_RID=au.USER_RID

END
GO
