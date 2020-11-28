--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts an Assortment Properties Basis
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_PROPERTIES_BASIS_INSERT]
	@HDR_RID int, 
	@BASIS_SEQ int, 
	@HN_RID int, 
	@FV_RID int, 
	@CDR_RID int, 
	@WEIGHT float
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO ASSORTMENT_PROPERTIES_BASIS
		(
		HDR_RID, 
		BASIS_SEQ, 
		HN_RID, 
		FV_RID, 
		CDR_RID, 
		WEIGHT
		)		
	VALUES
		(
		@HDR_RID, 
		@BASIS_SEQ, 
		@HN_RID, 
		@FV_RID, 
		@CDR_RID, 
		@WEIGHT 
		)		

    SELECT @@ROWCOUNT;
END

GO