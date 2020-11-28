--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Updates a base node row
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BASE_NODE_UPDATE]
	@HN_RID int,
	@BN_ID varchar(50),
	@BN_NAME varchar(50),
	@BN_DESCRIPTION varchar(250),
	@BN_PRODUCT_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE BASE_NODE WITH (ROWLOCK)
	SET 
		BN_ID = @BN_ID,
		BN_NAME = @BN_NAME,
		BN_DESCRIPTION = @BN_DESCRIPTION,
		BN_PRODUCT_TYPE = @BN_PRODUCT_TYPE
	WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
