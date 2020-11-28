--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Inserts a system basis label
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT]
	@SYSTEM_OPTION_RID int,
	@LABEL_TYPE int, 
	@LABEL_SEQ int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SYSTEM_OPTIONS_BASIS_LABELS
		(
		SYSTEM_OPTION_RID, 
		LABEL_TYPE, 
		LABEL_SEQ
		)
    VALUES
		(
		@SYSTEM_OPTION_RID, 
		@LABEL_TYPE, 
		@LABEL_SEQ
		)

	SELECT @@ROWCOUNT;

END
GO