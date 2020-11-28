CREATE VIEW [dbo].[VW_GET_PARTIAL_SHIPPED_HEADERS]
AS 
select * FROM VW_GET_HEADERS 
where ShippingComplete = 0 AND StyleIntransitUpdated = 1 AND ShippingStarted = 1 

GO
