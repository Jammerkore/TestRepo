--dv =============================================
--dv Create date:	1/27/2014
--dv Description:	Reads a Velocity Method 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_BASIS_READ_FROM_NODE]
	@BASIS_HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mvb.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, 
		au.USER_NAME 
    FROM METHOD_VELOCITY_BASIS mvb, 
		 METHOD m, 
		 APPLICATION_USER au 
    WHERE mvb.BASIS_HN_RID  = @BASIS_HN_RID
		  and mvb.METHOD_RID = m.METHOD_RID
		  and m.USER_RID = au.USER_RID
END
GO