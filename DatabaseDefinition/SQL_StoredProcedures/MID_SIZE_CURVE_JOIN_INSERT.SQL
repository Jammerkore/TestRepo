--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts a Size Curve Join
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_JOIN_INSERT]
	@SIZE_CURVE_RID INT,
	@SIZE_CODE_RID INT,
	@PERCENT FLOAT
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO SIZE_CURVE_JOIN
	(
	SIZE_CURVE_RID,
	SIZE_CODE_RID,
	[PERCENT]
	)
		VALUES 
		(
		@SIZE_CURVE_RID,
		@SIZE_CODE_RID,
		@PERCENT
		)
	SELECT @@ROWCOUNT
END

GO