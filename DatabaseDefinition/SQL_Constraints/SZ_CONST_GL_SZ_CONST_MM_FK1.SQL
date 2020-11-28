/* Add foreign key constraints to table "SIZE_CONSTRAINT_MINMAX".                             */
alter table "SIZE_CONSTRAINT_MINMAX"
	add constraint "SZ_CONST_GL_SZ_CONST_MM_FK1" foreign key (
		"SIZE_CONSTRAINT_RID",
		"SGL_RID")
	 references "SIZE_CONSTRAINT_GRPLVL" (
		"SIZE_CONSTRAINT_RID",
		"SGL_RID")  

go
