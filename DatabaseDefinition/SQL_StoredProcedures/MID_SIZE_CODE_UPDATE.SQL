--dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Updates Forecast Model table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CODE_UPDATE]
		@SIZE_CODE_RID int,
		@SIZE_CODE_ID varchar(50),
		@SIZE_CODE_PRIMARY varchar(100),
		@SIZE_CODE_SECONDARY varchar(100),
		@SIZE_CODE_PRODUCT_CATEGORY varchar(100)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SIZE_CODE 
	SET 
		SIZE_CODE_ID = @SIZE_CODE_ID,
		SIZE_CODE_PRIMARY = @SIZE_CODE_PRIMARY,
		SIZE_CODE_SECONDARY = @SIZE_CODE_SECONDARY,
        SIZE_CODE_PRODUCT_CATEGORY = @SIZE_CODE_PRODUCT_CATEGORY
	WHERE
        SIZE_CODE_RID = @SIZE_CODE_RID

	SELECT @@ROWCOUNT
END
GO