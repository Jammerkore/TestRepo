alter table "METHOD_GLOBAL_UNLOCK_GRP_LVL"
	add constraint "ST_GR_LV_MT_GL_UN_GR_LVL_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
