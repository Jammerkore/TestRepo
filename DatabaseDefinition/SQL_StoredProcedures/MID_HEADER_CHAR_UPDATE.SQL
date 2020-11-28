--dv =============================================
--dv Create date:	3/23/2015
--dv Description:	Updates a header characteristic value
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_UPDATE]
	@HC_RID INT,
	@TEXT_VALUE VARCHAR(50),
	@DATE_VALUE SMALLDATETIME,
	@NUMBER_VALUE FLOAT,
	@DOLLAR_VALUE FLOAT
AS
BEGIN
	SET NOCOUNT ON

	UPDATE HEADER_CHAR SET
		TEXT_VALUE = @TEXT_VALUE,
		DATE_VALUE = @DATE_VALUE,
		NUMBER_VALUE = @NUMBER_VALUE,
		DOLLAR_VALUE = @DOLLAR_VALUE
	WHERE HC_RID = @HC_RID

	SELECT @@ROWCOUNT;
END
GO


