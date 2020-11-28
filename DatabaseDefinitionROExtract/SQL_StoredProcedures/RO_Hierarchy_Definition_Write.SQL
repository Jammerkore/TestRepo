--dv =============================================
--dv Create date: 2/9/2018
--dv Description:	Write Hierarchy Definition Data
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Hierarchy_Definition_Write]
	@dt Hierarchy_Definition_Type READONLY
AS
BEGIN
SET NOCOUNT ON
--MERGE INTO Hierarchy_Definition with (ROWLOCK) AS TARGET
--USING @dt AS SOURCE
--on (SOURCE.[Type] = TARGET.[Type] and SOURCE.[Hierarchy_Level] = TARGET.[Hierarchy_Level])
--WHEN MATCHED THEN
--UPDATE 
--         SET TARGET.[ProductType] = COALESCE(SOURCE.[ProductType], TARGET.[ProductType]),
--             TARGET.[Hierarchy] = COALESCE(SOURCE.[Hierarchy], TARGET.[Hierarchy]),
--			 TARGET.[Parent] = COALESCE(SOURCE.[Parent], TARGET.[Parent]),
--			 TARGET.[Name] = COALESCE(SOURCE.[Name], TARGET.[Name]),
--             TARGET.[Description] = COALESCE(SOURCE.[Description], TARGET.[Description])
--WHEN NOT MATCHED BY TARGET THEN
--INSERT ([Type], [Hierarchy_Level])
--VALUES ( SOURCE.[Type], SOURCE.[Hierarchy_Level]);

truncate table Hierarchy_Definition

INSERT INTO Hierarchy_Definition with (ROWLOCK) ([Type], [Level_Number], [Hierarchy_Level])
SELECT [Type], [Level_Number], [Hierarchy_Level]
FROM @dt

END
GO





















