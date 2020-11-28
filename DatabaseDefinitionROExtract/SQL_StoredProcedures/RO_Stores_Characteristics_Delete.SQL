--dv =============================================
--dv Create date: 2/9/2018
--dv Description:	Delete Store Characteristics
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Stores_Characteristics_Delete]
	@Characteristic VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON

DELETE FROM Stores_Characteristics with (ROWLOCK) 
WHERE Characteristic = @Characteristic

END
GO





















