--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Inserts a dll image into the computations dll table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATIONS_DLL_INSERT]
	@VERSION varchar(30), 
	@DLL_IMAGE image, 
	@CREATION_DATE datetime
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO COMPUTATIONS_DLL 
	VALUES
		(
		@VERSION, 
		@DLL_IMAGE, 
		@CREATION_DATE
		)

	SELECT @@ROWCOUNT;

END
GO

