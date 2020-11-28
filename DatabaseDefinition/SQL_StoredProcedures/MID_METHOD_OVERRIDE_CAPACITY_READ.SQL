--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads capacity for the override method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_CAPACITY_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		moc.SGL_RID AS SglRID,
		sgl.SGL_ID AS SetName,
		moc.EXCEED_CAPACITY AS ExceedCapacity,
		moc.EXCEED_BY AS ExceedBy
	FROM METHOD_OVERRIDE_CAPACITY moc, 
		 STORE_GROUP_LEVEL sgl
	WHERE METHOD_RID = @METHOD_RID
		  AND moc.SGL_RID = sgl.SGL_RID
    ORDER BY SGL_SEQUENCE

END
GO