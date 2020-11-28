/* Add foreign key constraints to table "METHOD_VELOCITY_GRADE".                              */
alter table "METHOD_VELOCITY_GRADE"
	add constraint "MTHD_VEL_MTHD_VEL_GRD_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_VELOCITY" (
		"METHOD_RID")  

go
