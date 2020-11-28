alter table "SECURITY_USER_STORE"
	add constraint "STORES_SECURITY_USER_STORE_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
