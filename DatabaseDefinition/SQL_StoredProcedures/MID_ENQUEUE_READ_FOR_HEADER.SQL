--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns enqueue data based on the RID passed in for enqueue type of header and assortment
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_READ_FOR_HEADER]
	@RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ELOCK_TYPE_HEADER int = 4;
	DECLARE @ELOCK_TYPE_ASSORTMENT int = 34;

	SELECT 
		me.USER_RID, 
		me.RID, 
		COALESCE(me.OWNING_THREADID,0) as OWNING_THREADID, 
		COALESCE(me.OWNING_TRANID, 0) as OWNING_TRANID, 
		me.ENQUEUE_TIMESTAMP,
        case au.USER_FULLNAME when '' then au.USER_NAME else au.USER_FULLNAME end as USER_FULLNAME 
    FROM MID_ENQUEUE me, 
		 APPLICATION_USER au 
	WHERE ENQUEUE_TYPE IN (@ELOCK_TYPE_HEADER, @ELOCK_TYPE_ASSORTMENT)
		 AND RID = @RID
		 AND me.USER_RID=au.USER_RID

END
GO
