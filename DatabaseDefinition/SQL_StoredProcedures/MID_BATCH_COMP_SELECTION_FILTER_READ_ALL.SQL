--dv =============================================
--dv Create date:	05/13/2015
--dv Description:	Reads all rows fromm the Batch Comp Selection Filter Table 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BATCH_COMP_SELECTION_FILTER_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM BATCH_COMP_SELECTION_FILTER

END
GO