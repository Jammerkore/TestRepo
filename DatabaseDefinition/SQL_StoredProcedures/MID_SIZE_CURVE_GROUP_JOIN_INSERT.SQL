--dv =============================================
--dv Create date:	2/7/2013
--dv Description:	Inserts a Size Group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_GROUP_JOIN_INSERT]
	@SIZE_CURVE_GROUP_RID int,
	@SIZE_CURVE_RID int
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO SIZE_CURVE_GROUP_JOIN
		(
		SIZE_CURVE_GROUP_RID, 
		SIZE_CURVE_RID
		)
	VALUES 
		(
		@SIZE_CURVE_GROUP_RID, 
		@SIZE_CURVE_RID
		)
	SELECT @@ROWCOUNT
END
GO