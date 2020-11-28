--End TT#1111 - JScott - Size Curve build database deadlock
/* Add foreign key constraints to table "METHOD_GLOBAL_UNLOCK_GRP_LVL".                       */
alter table "METHOD_GLOBAL_UNLOCK_GRP_LVL"
	add constraint "MT_GL_U_MT_GL_U_GRP_LVL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_GLOBAL_UNLOCK" (
		"METHOD_RID")  

go
