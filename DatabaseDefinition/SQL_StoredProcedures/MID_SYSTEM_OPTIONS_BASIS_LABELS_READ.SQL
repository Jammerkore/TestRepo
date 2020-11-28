--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Reads system basis labels
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_BASIS_LABELS_READ]
	@SYSTEM_OPTION_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COALESCE(SYSTEM_OPTION_RID, -1) SYSTEM_OPTION_RID,
        LABEL_TYPE,
        LABEL_SEQ
    FROM SYSTEM_OPTIONS_BASIS_LABELS 
	WHERE SYSTEM_OPTION_RID = @SYSTEM_OPTION_RID
    ORDER BY LABEL_SEQ

END
GO