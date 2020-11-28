--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Returns a User Defined Assortment
--dv				Basis
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ASSORTMENT_BASIS_READ]
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		*
    FROM USER_ASSORTMENT_BASIS 
	WHERE USER_RID = @USER_RID
	
END
GO