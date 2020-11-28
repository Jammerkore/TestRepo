alter table "METHOD_GROUP_ALLOCATION"
	add constraint "METHOD_GROUP_ALLOCATION_ST_GRP_FK10" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go