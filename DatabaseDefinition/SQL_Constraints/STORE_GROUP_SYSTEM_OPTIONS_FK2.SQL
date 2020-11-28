alter table "SYSTEM_OPTIONS"
	add constraint "STORE_GROUP_SYSTEM_OPTIONS_FK2" foreign key (
		"DEFAULT_ALLOC_SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
