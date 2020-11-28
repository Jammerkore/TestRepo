--dv =============================================
--dv Modified date: 4/18/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith- DC Fulfillment
--dv =============================================   

alter table "METHOD_CREATE_MASTER_HEADERS_MERCHANDISE"
	add constraint "METHOD_CREATE_MASTER_HEADERS_MERCHANDISE_FK2" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
