alter table "METHOD_OVERRIDE_CAPACITY"
	add constraint "MTHD_OVRD_MTHD_OVRD_CAP_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_OVERRIDE" (
		"METHOD_RID")  

go
