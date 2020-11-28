--dv =============================================
--dv Create date:	1/27/2014
--dv Description:	Reads a Velocity Method 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_READ_FROM_NODE]
	@OTS_PLAN_HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mv.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, 
		au.USER_NAME 
    FROM METHOD_VELOCITY mv, 
		 METHOD m, 
		 APPLICATION_USER au 
    WHERE mv.OTS_PLAN_HN_RID   = @OTS_PLAN_HN_RID
		  and mv.METHOD_RID = m.METHOD_RID
		  and m.USER_RID = au.USER_RID
END
GO