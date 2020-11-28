/* Add foreign key constraints to table "SYSTEM_OPTIONS".                                     */
alter table "SYSTEM_OPTIONS"
	add constraint "STORE_GROUP_SYSTEM_OPTIONS_FK1" foreign key (
		"DEFAULT_OTS_SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
