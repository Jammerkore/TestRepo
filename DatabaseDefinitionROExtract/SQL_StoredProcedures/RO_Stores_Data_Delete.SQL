--dv =============================================
--dv Create date: 3/26/2018
--dv Description:	Delete Store Data
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Stores_Data_Delete]
	@dt Stores_Data_Type READONLY
AS
BEGIN

    SET NOCOUNT ON

    DELETE [dbo].[Stores_Data] WITH (ROWLOCK)
      FROM [dbo].[Stores_Data] t
    INNER JOIN @dt d ON d.ID = t.ID

	DELETE [dbo].[Stores_Characteristics] WITH (ROWLOCK)
      FROM [dbo].[Stores_Characteristics] t
    INNER JOIN @dt d ON d.ID = t.ID

	DELETE [dbo].[Planning_Stores] WITH (ROWLOCK)
      FROM [dbo].[Planning_Stores] t
    INNER JOIN @dt d ON d.ID = t.Store

	DELETE [dbo].[Planning_Stores_Total] WITH (ROWLOCK)
      FROM [dbo].[Planning_Stores_Total] t
    INNER JOIN @dt d ON d.ID = t.Store

	SELECT @@ROWCOUNT

END
GO





















