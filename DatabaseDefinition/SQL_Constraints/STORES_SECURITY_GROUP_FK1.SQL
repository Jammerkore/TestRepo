alter table "SECURITY_GROUP_STORE"
	add constraint "STORES_SECURITY_GROUP_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
