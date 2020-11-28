--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Inserts a row into the master header table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_MASTER_HEADER_INSERT] 
	@SUBORD_HDR_RID int,
	@MASTER_HDR_RID int,
	@MASTER_COMPONENT int,
	@MASTER_PACK_RID int,
	@MASTER_BC_RID int,
	@SUBORD_COMPONENT int,
	@SUBORD_PACK_RID int,
	@SUBORD_BC_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO MASTER_HEADER 
		(
		SUBORD_HDR_RID, 
		MASTER_HDR_RID, 
		MASTER_COMPONENT, 
		MASTER_PACK_RID, 
		MASTER_BC_RID,
        SUBORD_COMPONENT, 
		SUBORD_PACK_RID, 
		SUBORD_BC_RID
		)
    VALUES 
		(
		@SUBORD_HDR_RID, 
		@MASTER_HDR_RID,
        @MASTER_COMPONENT,	
		@MASTER_PACK_RID, 
		@MASTER_BC_RID, 
        @SUBORD_COMPONENT, 
		@SUBORD_PACK_RID, 
		@SUBORD_BC_RID
		)

	SELECT @@ROWCOUNT;

END
GO
