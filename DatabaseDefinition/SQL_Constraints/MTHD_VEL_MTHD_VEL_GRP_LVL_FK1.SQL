/* Add foreign key constraints to table "METHOD_VELOCITY_GROUP_LEVEL".                        */
alter table "METHOD_VELOCITY_GROUP_LEVEL"
	add constraint "MTHD_VEL_MTHD_VEL_GRP_LVL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_VELOCITY" (
		"METHOD_RID")  

go
