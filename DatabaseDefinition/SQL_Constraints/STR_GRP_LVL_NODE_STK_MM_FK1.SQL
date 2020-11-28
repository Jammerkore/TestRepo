alter table "NODE_STOCK_MIN_MAX"
	add constraint "STR_GRP_LVL_NODE_STK_MM_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
