--dv =============================================
--dv Create date:	8/14/2014
--dv Description:	Inserts a group allocation method store grade value
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_INSERT]
	@METHOD_RID int,
	@BOUNDARY int, 
	@MINIMUM_GROUP int,
	@MAXIMUM_GROUP int,
	@MINIMUM_HEADER int,
	@MAXIMUM_HEADER int,
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES 
		(
		METHOD_RID, 
		BOUNDARY, 
		MINIMUM_GROUP,
		MAXIMUM_GROUP,
		MINIMUM_HEADER,
		MAXIMUM_HEADER,
		SGL_RID
		)
	VALUES
		(
		@METHOD_RID, 
		@BOUNDARY, 
		@MINIMUM_GROUP,
		@MAXIMUM_GROUP,
		@MINIMUM_HEADER,
		@MAXIMUM_HEADER,
		@SGL_RID
		)

	SELECT @@ROWCOUNT;
END
GO