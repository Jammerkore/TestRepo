--dv =============================================
--dv Create date:	3/15/2015
--dv Description:	Returns enqueue data based on the type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_READ_FROM_TYPE_ANY]
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
		 AND me.USER_RID=au.USER_RID

END
GO
