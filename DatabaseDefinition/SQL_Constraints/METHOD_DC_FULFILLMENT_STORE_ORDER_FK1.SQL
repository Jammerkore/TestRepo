--dv =============================================
--dv Modified date: 4/18/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith- DC Fulfillment
--dv =============================================   

alter table "METHOD_DC_FULFILLMENT_STORE_ORDER"
	add constraint "METHOD_DC_FULFILLMENT_STORE_ORDER_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_DC_FULFILLMENT" (
		"METHOD_RID")  

go
