/* Add foreign key constraints to table "USER_METHOD_PREF".                                   */
alter table "USER_METHOD_PREF"
	add constraint "METHOD_USER_METHOD_PREF_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
