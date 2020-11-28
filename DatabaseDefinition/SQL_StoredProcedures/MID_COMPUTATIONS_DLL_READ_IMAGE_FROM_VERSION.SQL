--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads the dll image based on version
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION]
	@VERSION varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		DLL_IMAGE 
	FROM COMPUTATIONS_DLL 
	WHERE VERSION = @VERSION

END
GO
