--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Reads rollup method basis details from the method provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_ROLLUP_BASIS_DETAIL_READ_FROM_METHOD] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
    FROM METHOD_ROLLUP MR WITH (NOLOCK)
    INNER JOIN METHOD_ROLLUP_BASIS_DETAIL MRBD WITH (NOLOCK) on MRBD.METHOD_RID = MR.METHOD_RID
    INNER JOIN METHOD M WITH (NOLOCK) on M.METHOD_RID = MR.METHOD_RID
    INNER JOIN APPLICATION_USER AU WITH (NOLOCK) on AU.USER_RID = M.USER_RID
    WHERE MR.METHOD_RID = @METHOD_RID

END
GO