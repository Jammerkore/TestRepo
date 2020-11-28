alter table "NODE_SIZE_CURVE_TOLERANCE" 
	add constraint "ND_SZ_CRV_TOLERANCE_FK1" foreign key ("HN_RID")
	 references "HIERARCHY_NODE" ("HN_RID")  

go
