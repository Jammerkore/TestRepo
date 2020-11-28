--dv =============================================
--dv Create date:	5/8/2017
--dv Description:	Inserts a header pack association row
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_ASSOCIATION_INSERT]
	@HDR_PACK_RID int,
	@SEQ int,
	@HDR_RID int,
	@ASSOCIATED_PACK_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO HEADER_PACK_ASSOCIATION (
		HDR_PACK_RID,
		SEQ,
		HDR_RID,
		ASSOCIATED_PACK_RID
		)
	VALUES (
		@HDR_PACK_RID,
		@SEQ,
		@HDR_RID,
		@ASSOCIATED_PACK_RID
		)

	SELECT @@ROWCOUNT;

END
GO
