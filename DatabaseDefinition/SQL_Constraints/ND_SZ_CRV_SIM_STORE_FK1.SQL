alter table "NODE_SIZE_CURVE_SIMILAR_STORE" 
	add constraint "ND_SZ_CRV_SIM_STORE_FK1" foreign key ("HN_RID")
	 references "HIERARCHY_NODE" ("HN_RID")  

go
