/* Add foreign key constraints to table "SIZE_CONSTRAINT_MODEL".                              */
alter table "SIZE_CONSTRAINT_MODEL"
	add constraint "STORE_GROUP_SZ_CONST_MODEL_FK1" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
