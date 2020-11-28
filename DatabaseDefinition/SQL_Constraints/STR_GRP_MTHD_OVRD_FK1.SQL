alter table "METHOD_OVERRIDE"
	add constraint "STR_GRP_MTHD_OVRD_FK1" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
