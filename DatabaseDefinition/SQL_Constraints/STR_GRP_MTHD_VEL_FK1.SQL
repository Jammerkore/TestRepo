alter table "METHOD_VELOCITY"
	add constraint "STR_GRP_MTHD_VEL_FK1" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
