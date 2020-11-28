--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Reads the hierarchy rid from the base id
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BASE_NODE_READ_FROM_ID]
	@BN_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT HN_RID 
	FROM BASE_NODE WITH (NOLOCK)
	WHERE BN_ID=@BN_ID 

END
GO
