--dv =============================================
--dv Create date: 2/9/2018
--dv Description:	Delete Hierarchy Data
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Hierarchy_Data_Delete]
	@dt Hierarchy_Data_Type READONLY
AS
BEGIN

    SET NOCOUNT ON

    DELETE [dbo].[Hierarchy_Data] WITH (ROWLOCK)
      FROM [dbo].[Hierarchy_Data] t
    INNER JOIN @dt d ON d.ID = t.ID

	DELETE [dbo].[Planning_Attributes] WITH (ROWLOCK)
      FROM [dbo].[Planning_Attributes] t
    INNER JOIN @dt d ON d.ID = t.Merchandise

	DELETE [dbo].[Planning_Attributes_Total] WITH (ROWLOCK)
      FROM [dbo].[Planning_Attributes_Total] t
    INNER JOIN @dt d ON d.ID = t.Merchandise

	DELETE [dbo].[Planning_Chain] WITH (ROWLOCK)
      FROM [dbo].[Planning_Chain] t
    INNER JOIN @dt d ON d.ID = t.Merchandise

	DELETE [dbo].[Planning_Chain_Total] WITH (ROWLOCK)
      FROM [dbo].[Planning_Chain_Total] t
    INNER JOIN @dt d ON d.ID = t.Merchandise

	DELETE [dbo].[Planning_Stores] WITH (ROWLOCK)
      FROM [dbo].[Planning_Stores] t
    INNER JOIN @dt d ON d.ID = t.Merchandise

	DELETE [dbo].[Planning_Stores_Total] WITH (ROWLOCK)
      FROM [dbo].[Planning_Stores_Total] t
    INNER JOIN @dt d ON d.ID = t.Merchandise


	SELECT @@ROWCOUNT

END
GO




















