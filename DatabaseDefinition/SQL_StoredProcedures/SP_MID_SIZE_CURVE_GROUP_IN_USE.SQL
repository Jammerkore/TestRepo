--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Reads objects that use the size curve group
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_SIZE_CURVE_GROUP_IN_USE] 
	@inSizeCurveGroup VARCHAR(250)
AS
BEGIN
	DECLARE @sizeCurveGroupRid INT

	SELECT @sizeCurveGroupRid = SIZE_CURVE_GROUP_RID
	FROM SIZE_CURVE_GROUP
	WHERE SIZE_CURVE_GROUP_NAME = @inSizeCurveGroup

	SELECT @inSizeCurveGroup 'Size Curve Group',
		@sizeCurveGroupRid 'Size Curve Group Key'

	SELECT h.HDR_ID '...is on Header ID'
	FROM HEADER_BULK_COLOR_SIZE_NEED hb,
		HEADER h
	WHERE hb.HDR_RID = h.HDR_RID
		AND SIZE_CURVE_GROUP_RID = @sizeCurveGroupRid
	
	UNION ALL
	
	SELECT h.HDR_ID
	FROM HEADER_SIZE_NEED hb,
		HEADER h
	WHERE hb.HDR_RID = h.HDR_RID
		AND SIZE_CURVE_GROUP_RID = @sizeCurveGroupRid

	SELECT SIZE_CONSTRAINT_NAME '...is on Size Constraint Model'
	FROM SIZE_CONSTRAINT_MODEL
	WHERE SIZE_CURVE_GROUP_RID = @sizeCurveGroupRid

	SELECT m.METHOD_NAME '...is on Size Need Method'
	FROM METHOD_SIZE_NEED msn,
		METHOD m
	WHERE msn.METHOD_RID = m.METHOD_RID
		AND msn.SIZE_CURVE_GROUP_RID = @sizeCurveGroupRid

	SELECT m.METHOD_NAME '...is on Fill Size Holes Method'
	FROM METHOD_FILL_SIZE_HOLES msn,
		METHOD m
	WHERE msn.METHOD_RID = m.METHOD_RID
		AND msn.SIZE_CURVE_GROUP_RID = @sizeCurveGroupRid

	SELECT m.METHOD_NAME '...is on Size Basis Method'
	FROM METHOD_SIZE_BASIS_ALLOCATION msn,
		METHOD m
	WHERE msn.METHOD_RID = m.METHOD_RID
		AND msn.SIZE_CURVE_GROUP_RID = @sizeCurveGroupRid

	SELECT m.METHOD_NAME '...is on Size Need Allocation Method'
	FROM METHOD_SIZE_NEED_ALLOCATION msn,
		METHOD m
	WHERE msn.METHOD_RID = m.METHOD_RID
		AND msn.SIZE_CURVE_GROUP_RID = @sizeCurveGroupRid
END
GO


