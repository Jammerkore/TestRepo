alter table "METHOD_VELOCITY_MATRIX"
	add constraint "MT_VEL_GRP_LVL_MT_VEL_MTRX_FK1" foreign key (
		"METHOD_RID",
		"SGL_RID")
	 references "METHOD_VELOCITY_GROUP_LEVEL" (
		"METHOD_RID",
		"SGL_RID")  

go
