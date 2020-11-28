--dv =============================================
--dv Modified date: 4/18/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith- DC Fulfillment
--dv =============================================   

alter table "METHOD_CREATE_MASTER_HEADERS_OVERRIDE"
	add constraint "METHOD_CREATE_MASTER_HEADERS_OVERRIDE_FK2" foreign key (
		"HCG_RID")
	 references "HEADER_CHAR_GROUP" (
		"HCG_RID")  

go
