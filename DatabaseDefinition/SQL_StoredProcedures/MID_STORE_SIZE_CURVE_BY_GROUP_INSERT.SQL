--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts a Size Curve by Group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_SIZE_CURVE_BY_GROUP_INSERT]
	@ST_RID INT,
	@SIZE_CURVE_GROUP_RID INT,
	@SIZE_CURVE_RID INT
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO STORE_SIZE_CURVE_BY_GROUP
	(
	ST_RID,
	SIZE_CURVE_GROUP_RID,
	SIZE_CURVE_RID
	)
		VALUES 
		(
		@ST_RID,
		@SIZE_CURVE_GROUP_RID,
		@SIZE_CURVE_RID
		)

	SELECT @@ROWCOUNT

END

GO