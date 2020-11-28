--dv =============================================
--dv Create date:	10/30/2013
--dv Description:	Inserts a row into the user allocation basis table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ALLOCATION_BASIS_INSERT]
	@USER_RID int,
	@BASIS_SEQUENCE int, 
	@BASIS_HN_RID int, 
	@BASIS_PH_RID int,
	@BASIS_PHL_SEQUENCE int, 
	@BASIS_FV_RID int, 
	@CDR_RID int, 
	@SALES_WEIGHT float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO USER_ALLOCATION_BASIS 
		(
		USER_RID, 
		BASIS_SEQUENCE, 
		BASIS_HN_RID, 
		BASIS_PH_RID,
		BASIS_PHL_SEQUENCE, 
		BASIS_FV_RID, 
		CDR_RID, 
		SALES_WEIGHT
		) 
    VALUES
		(
		@USER_RID, 
		@BASIS_SEQUENCE, 
		@BASIS_HN_RID, 
		@BASIS_PH_RID,
		@BASIS_PHL_SEQUENCE, 
		@BASIS_FV_RID, 
		@CDR_RID, 
		@SALES_WEIGHT
		) 

	SELECT @@ROWCOUNT;

END
GO
