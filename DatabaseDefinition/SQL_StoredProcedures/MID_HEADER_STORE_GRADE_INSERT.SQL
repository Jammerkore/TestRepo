--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Inserts a row into the header store grade table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_STORE_GRADE_INSERT]
	@HDR_RID int,
	@BOUNDARY int,
	@GRADE_CODE varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO HEADER_STORE_GRADE WITH (ROWLOCK) (HDR_RID, BOUNDARY, GRADE_CODE)
    VALUES (@HDR_RID, @BOUNDARY, @GRADE_CODE)

	SELECT @@ROWCOUNT;

END
GO
