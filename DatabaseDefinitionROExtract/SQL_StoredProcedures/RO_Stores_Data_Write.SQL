--dv =============================================
--dv Create date: 2/9/2018
--dv Description:	Write Store Data
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Stores_Data_Write]
	@dt Stores_Data_Type READONLY
AS
BEGIN
SET NOCOUNT ON
MERGE INTO Stores_Data with (ROWLOCK) AS TARGET
USING @dt AS SOURCE
on (SOURCE.[ID] = TARGET.[ID])
WHEN MATCHED THEN
UPDATE 
         SET TARGET.[StoreDisplay] = COALESCE(SOURCE.[StoreDisplay], TARGET.[StoreDisplay]),
             TARGET.[Name] = COALESCE(SOURCE.[Name], TARGET.[Name]),
			 TARGET.[Description] = COALESCE(SOURCE.[Description], TARGET.[Description]),
			 TARGET.[City] = COALESCE(SOURCE.[City], TARGET.[City]),
			 TARGET.[State] = COALESCE(SOURCE.[State], TARGET.[State]),
			 TARGET.[SellingSquareFeet] = COALESCE(SOURCE.[SellingSquareFeet], TARGET.[SellingSquareFeet]),
			 TARGET.[SellingOpenDate] = COALESCE(SOURCE.[SellingOpenDate], TARGET.[SellingOpenDate]),
			 TARGET.[SellingCloseDate] = COALESCE(SOURCE.[SellingCloseDate], TARGET.[SellingCloseDate]),
			 TARGET.[StockOpenDate] = COALESCE(SOURCE.[StockOpenDate], TARGET.[StockOpenDate]),
			 TARGET.[StockCloseDate] = COALESCE(SOURCE.[StockCloseDate], TARGET.[StockCloseDate]),
			 TARGET.[ActiveIndicator] = COALESCE(SOURCE.[ActiveIndicator], TARGET.[ActiveIndicator]),
			 TARGET.[Status] = COALESCE(SOURCE.[Status], TARGET.[Status]),
			 TARGET.[LeadTime] = COALESCE(SOURCE.[LeadTime], TARGET.[LeadTime]),
			 TARGET.[ShipDate] = COALESCE(SOURCE.[ShipDate], TARGET.[ShipDate]),
             TARGET.[VSWID] = COALESCE(SOURCE.[VSWID], TARGET.[VSWID])
WHEN NOT MATCHED BY TARGET THEN
INSERT ([ID], [StoreDisplay], [Name], [Description], [City], [State], [SellingSquareFeet],	[SellingOpenDate], [SellingCloseDate], [StockOpenDate],	[StockCloseDate], [ActiveIndicator], [Status], [LeadTime], [ShipDate], [VSWID])
VALUES ( SOURCE.[ID], SOURCE.[StoreDisplay], SOURCE.[Name], SOURCE.[Description], SOURCE.[City], SOURCE.[State], SOURCE.[SellingSquareFeet],	SOURCE.[SellingOpenDate], SOURCE.[SellingCloseDate], SOURCE.[StockOpenDate],	
SOURCE.[StockCloseDate], SOURCE.[ActiveIndicator], SOURCE.[Status], SOURCE.[LeadTime], SOURCE.[ShipDate], SOURCE.[VSWID]);

END
GO





















