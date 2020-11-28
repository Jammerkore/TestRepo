CREATE VIEW [dbo].[VW_FIND_HEADERS_WITH_ACTIVE_INTRANSIT]
AS
select * from VW_GET_HEADERS 
where ShippingComplete = 0 and StyleIntransitUpdated = 1 

GO
