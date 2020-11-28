--dv =============================================
--dv Create date:	3/19/2015
--dv Description:	Updates a store characteristic value
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_UPDATE]
	@SC_RID INT,
	@TEXT_VALUE VARCHAR(50),
	@DATE_VALUE SMALLDATETIME,
	@NUMBER_VALUE FLOAT,
	@DOLLAR_VALUE FLOAT
AS
BEGIN
	SET NOCOUNT ON

	UPDATE STORE_CHAR SET
		TEXT_VALUE = @TEXT_VALUE,
		DATE_VALUE = @DATE_VALUE,
		NUMBER_VALUE = @NUMBER_VALUE,
		DOLLAR_VALUE = @DOLLAR_VALUE
	WHERE SC_RID = @SC_RID

	SELECT @@ROWCOUNT;
END
GO


