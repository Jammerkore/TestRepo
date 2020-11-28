--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Inserts a row into the reclass rejected header table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_RECLASS_REJECTED_HEADER_INSERT] 
	@PROCESS_RID int,
	@HDR_RID int,
	@HDR_ID varchar(100),
	@HDR_STATUS varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO RECLASS_REJECTED_HEADER (PROCESS_RID, HDR_RID, HDR_ID, HDR_STATUS)
    VALUES (@PROCESS_RID, @HDR_RID, @HDR_ID, @HDR_STATUS)

	SELECT @@ROWCOUNT;

END
GO
