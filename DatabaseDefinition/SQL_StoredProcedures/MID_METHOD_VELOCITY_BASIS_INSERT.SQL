--dv =============================================
--dv Create date:	1/27/2014
--dv Description:	Inserts a Velocity Basis Method item
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_BASIS_INSERT]
	@METHOD_RID int,
	@BASIS_SEQUENCE int,
	@BASIS_HN_RID int,
	@BASIS_FV_RID int,
	@CDR_RID int,
	@SALES_WEIGHT float,
	@BASIS_PH_RID int,
	@BASIS_PHL_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_VELOCITY_BASIS  
		(
		METHOD_RID, 
		BASIS_SEQUENCE,
		BASIS_HN_RID,
		BASIS_FV_RID,
		CDR_RID,
		SALES_WEIGHT,
		BASIS_PH_RID,
		BASIS_PHL_SEQUENCE
		)
	VALUES
		(
		@METHOD_RID, 
		@BASIS_SEQUENCE,
		@BASIS_HN_RID,
		@BASIS_FV_RID,
		@CDR_RID,
		@SALES_WEIGHT,
		@BASIS_PH_RID,
		@BASIS_PHL_SEQUENCE
		)

	SELECT @@ROWCOUNT;
END
GO