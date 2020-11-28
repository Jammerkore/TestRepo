/* Add foreign key constraints to table "METHOD_VELOCITY_MATRIX".                             */
alter table "METHOD_VELOCITY_MATRIX"
	add constraint "MTHD_VEL_GRD_MTHD_VEL_MTRX_FK1" foreign key (
		"METHOD_RID",
		"BOUNDARY")
	 references "METHOD_VELOCITY_GRADE" (
		"METHOD_RID",
		"BOUNDARY")  

go
