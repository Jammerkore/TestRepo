/* Add foreign key constraints to table "COMPUTATION_MODEL_ENTRY".                            */
alter table "COMPUTATION_MODEL_ENTRY"
	add constraint "COMP_MDL_COMP_MDL_ENTRY_FK1" foreign key (
		"COMP_MODEL_RID")
	 references "COMPUTATION_MODEL" (
		"COMP_MODEL_RID")  

go
