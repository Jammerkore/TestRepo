--dv =============================================
--dv Create date: 1/17/2014
--dv Description: Inserts a group level node function
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GROUP_LEVEL_NODE_FUNCTION_INSERT] 
	@METHOD_RID int, 
	@SGL_RID int,
	@HN_RID int, 
	@APPLY_MIN_MAXES_IND char(1),
	@MIN_MAXES_INHERIT_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO GROUP_LEVEL_NODE_FUNCTION 
		(
		METHOD_RID, 
		SGL_RID, 
		HN_RID, 
		APPLY_MIN_MAXES_IND, 
		MIN_MAXES_INHERIT_TYPE
		)
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID, 
		@HN_RID, 
		@APPLY_MIN_MAXES_IND, 
		@MIN_MAXES_INHERIT_TYPE
		)

	SELECT @@ROWCOUNT;

END
GO