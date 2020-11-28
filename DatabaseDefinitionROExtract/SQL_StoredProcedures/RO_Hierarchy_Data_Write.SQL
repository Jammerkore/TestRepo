--dv =============================================
--dv Create date: 2/9/2018
--dv Description:	Write Hierarchy Data
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Hierarchy_Data_Write]
	@dt Hierarchy_Data_Type READONLY
AS
BEGIN
SET NOCOUNT ON
MERGE INTO Hierarchy_Data with (ROWLOCK) AS TARGET
USING @dt AS SOURCE
on (SOURCE.[ID] = TARGET.[ID])
WHEN MATCHED THEN
UPDATE 
         SET TARGET.[NodeDisplay] = COALESCE(SOURCE.[NodeDisplay], TARGET.[NodeDisplay]),
             TARGET.[ProductType] = COALESCE(SOURCE.[ProductType], TARGET.[ProductType]),
             TARGET.[Hierarchy] = COALESCE(SOURCE.[Hierarchy], TARGET.[Hierarchy]),
			 TARGET.[Parent] = COALESCE(SOURCE.[Parent], TARGET.[Parent]),
			 TARGET.[Name] = COALESCE(SOURCE.[Name], TARGET.[Name]),
             TARGET.[Description] = COALESCE(SOURCE.[Description], TARGET.[Description])
			 
WHEN NOT MATCHED BY TARGET THEN
INSERT ([ID], [NodeDisplay], [ProductType], [Hierarchy], [Parent], [Name], [Description])
VALUES ( SOURCE.[ID], SOURCE.[NodeDisplay], SOURCE.[ProductType], SOURCE.[Hierarchy], SOURCE.[Parent], SOURCE.[Name], SOURCE.[Description]);

END
GO




















