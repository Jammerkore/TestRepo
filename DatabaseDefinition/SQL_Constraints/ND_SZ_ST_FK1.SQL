alter table "NODE_SIZE_SELLTHRU"
	add constraint "ND_SZ_ST_FK1" foreign key ("HN_RID")
	 references "HIERARCHY_NODE" ("HN_RID")  

go
