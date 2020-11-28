/* Add foreign key constraints to table "METHOD_VELOCITY_BASIS".                              */
alter table "METHOD_VELOCITY_BASIS"
	add constraint "MTHD_VEL_MTHD_VEL_BAS_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_VELOCITY" (
		"METHOD_RID")  

go
