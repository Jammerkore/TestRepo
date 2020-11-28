alter table "METHOD_GROUP_ALLOCATION"
	add constraint "METHOD_GROUP_ALLOCATION_ST_GRP_FK11" foreign key (
		"STORE_GRADES_SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go 