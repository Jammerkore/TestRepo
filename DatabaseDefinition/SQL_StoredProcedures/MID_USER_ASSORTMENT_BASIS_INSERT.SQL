--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Inserts Assortment User Basis
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ASSORTMENT_BASIS_INSERT]
	@USER_RID int, 
	@BASIS_SEQ int, 
	@HN_RID int, 
	@FV_RID int, 
	@CDR_RID int, 
	@WEIGHT float
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO USER_ASSORTMENT_BASIS
		(
		USER_RID, 
		BASIS_SEQ, 
		HN_RID, 
		FV_RID, 
		CDR_RID, 
		WEIGHT
		)		
	VALUES
		(
		@USER_RID, 
		@BASIS_SEQ, 
		@HN_RID, 
		@FV_RID, 
		@CDR_RID, 
		@WEIGHT
		)		

    SELECT @@ROWCOUNT;
END

GO