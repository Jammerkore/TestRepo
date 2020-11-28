--dv =============================================
--dv Create date:	1/27/2014
--dv Description:	Inserts a Velocity Grade Method item
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_GRADE_INSERT]
	@METHOD_RID int,
	@BOUNDARY int,
	@GRADE_CODE varchar(20),
	@MINIMUM_STOCK int,
	@MAXIMUM_STOCK int,
	@MINIMUM_AD int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_VELOCITY_GRADE  
		(
		METHOD_RID, 
		BOUNDARY,
		GRADE_CODE,
		MINIMUM_STOCK,
		MAXIMUM_STOCK,
		MINIMUM_AD
		)
	VALUES
		(
		@METHOD_RID, 
		@BOUNDARY,
		@GRADE_CODE,
		@MINIMUM_STOCK,
		@MAXIMUM_STOCK,
		@MINIMUM_AD
		)

	SELECT @@ROWCOUNT;
END
GO