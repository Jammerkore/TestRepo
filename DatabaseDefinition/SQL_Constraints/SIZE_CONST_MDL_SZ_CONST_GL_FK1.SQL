/* Add foreign key constraints to table "SIZE_CONSTRAINT_GRPLVL".                             */
alter table "SIZE_CONSTRAINT_GRPLVL"
	add constraint "SIZE_CONST_MDL_SZ_CONST_GL_FK1" foreign key (
		"SIZE_CONSTRAINT_RID")
	 references "SIZE_CONSTRAINT_MODEL" (
		"SIZE_CONSTRAINT_RID")  

go
