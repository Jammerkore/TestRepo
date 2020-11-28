--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Inserts a header release type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_TYPE_RELEASE_INSERT]
		@HEADER_TYPE int, 
		@RELEASE_HEADER_TYPE_IND char(1)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO HEADER_TYPE_RELEASE
		(
		HEADER_TYPE, 
		RELEASE_HEADER_TYPE_IND
		)
	VALUES 
		(
		@HEADER_TYPE, 
		@RELEASE_HEADER_TYPE_IND
		)

	SELECT @@ROWCOUNT;
END
GO