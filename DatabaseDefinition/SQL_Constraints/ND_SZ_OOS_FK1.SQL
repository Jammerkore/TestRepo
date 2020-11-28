alter table "NODE_SIZE_OUTOFSTOCK"
	add constraint "ND_SZ_OOS_FK1" foreign key ("HN_RID")
	 references "HIERARCHY_NODE" ("HN_RID")  

go
