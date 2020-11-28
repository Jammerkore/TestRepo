--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Updates application text
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_UPDATE]
    @TEXT_CODE int,
	@TEXT_VALUE varchar(250), 
	@TEXT_LEVEL int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE APPLICATION_TEXT 
	SET
		TEXT_VALUE = @TEXT_VALUE,
		TEXT_LEVEL = @TEXT_LEVEL
	WHERE TEXT_CODE = @TEXT_CODE

	SELECT @@ROWCOUNT;
END
GO