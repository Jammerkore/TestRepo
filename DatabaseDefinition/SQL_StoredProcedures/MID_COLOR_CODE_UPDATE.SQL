--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Updates a color code row based on the COLOR_CODE_RID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_CODE_UPDATE]
	@COLOR_CODE_RID int, --Color Code to update
	@COLOR_CODE_ID varchar(50),
	@COLOR_CODE_NAME varchar(250),
	@COLOR_CODE_GROUP varchar(250),
	@VIRTUAL_IND char(1),
	@PURPOSE int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE COLOR_CODE SET
		COLOR_CODE_ID = @COLOR_CODE_ID,
		COLOR_CODE_NAME = @COLOR_CODE_NAME,
		COLOR_CODE_GROUP = @COLOR_CODE_GROUP,
		VIRTUAL_IND = @VIRTUAL_IND,
		PURPOSE = @PURPOSE
	WHERE COLOR_CODE_RID = @COLOR_CODE_RID

	SELECT @@ROWCOUNT;

END
GO
